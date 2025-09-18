using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services;
using ChipSys.Admin.Resources;
using ChipSys.Admin.Repositories;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Client.Contracts.Services.Client;
using ChipSys.Client.Contracts.Services.Client.Dto;
using ChipSys.Admin.Domain.Admin;
using ChipSys.Admin.Core;
using ChipSys.Common.Helpers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace ChipSys.Admin.Services.Client;

/// <summary>
/// 支付宝支付服务
/// </summary>
[Order(110)]
[DynamicApi(Area = "client")]
public class AlipayPaymentService : BaseService, IPaymentProviderService, IDynamicApi
{
    private readonly AdminRepositoryBase<PaymentConfigEntity> _paymentConfigRep;
    private readonly AdminLocalizer _adminLocalizer;

    public AlipayPaymentService(
        AdminRepositoryBase<PaymentConfigEntity> paymentConfigRep,
        AdminLocalizer adminLocalizer)
    {
        _paymentConfigRep = paymentConfigRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 创建支付宝支付订单
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="amount"></param>
    /// <param name="subject"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public async Task<PaymentCreateOutput> CreateAlipayOrderAsync(string orderId, int amount, string subject, string? description = null)
    {
        // 获取支付宝配置
        var config = await GetAlipayConfigAsync();
        if (config == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["支付宝配置不存在"]);
        }

        // 构建请求参数
        var parameters = new Dictionary<string, string>
        {
            ["app_id"] = config.AppId,
            ["method"] = "alipay.trade.precreate",
            ["charset"] = "utf-8",
            ["sign_type"] = "RSA2",
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            ["version"] = "1.0",
            ["notify_url"] = config.NotifyUrl,
            ["biz_content"] = JsonSerializer.Serialize(new
            {
                out_trade_no = orderId,
                total_amount = (amount / 100.0).ToString("F2"),
                subject = subject,
                body = description ?? subject
            })
        };

        // 生成签名
        var sign = GenerateAlipaySign(parameters, config.PrivateKey);
        parameters["sign"] = sign;

        // 调用支付宝API
        var response = await CallAlipayApiAsync(parameters, config.Environment);
        
        if (response.ContainsKey("alipay_trade_precreate_response"))
        {
            var precreateResponse = JsonSerializer.Deserialize<JsonElement>(response["alipay_trade_precreate_response"]);
            
            if (precreateResponse.TryGetProperty("code", out var code) && code.GetString() == "10000")
            {
                var qrCode = precreateResponse.GetProperty("qr_code").GetString();
                return new PaymentCreateOutput
                {
                    OrderId = orderId,
                    QrCode = qrCode,
                    Status = "pending",
                    ExpireTime = DateTime.Now.AddMinutes(30)
                };
            }
            else
            {
                var msg = precreateResponse.TryGetProperty("msg", out var message) ? message.GetString() : "未知错误";
                throw ResultOutput.Exception($"支付宝预下单失败: {msg}");
            }
        }

        throw ResultOutput.Exception("支付宝API调用失败");
    }

    /// <summary>
    /// 创建微信支付订单
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="amount"></param>
    /// <param name="subject"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public async Task<PaymentCreateOutput> CreateWechatOrderAsync(string orderId, int amount, string subject, string? description = null)
    {
        // 获取微信支付配置
        var config = await GetWechatConfigAsync();
        if (config == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["微信支付配置不存在"]);
        }

        // 构建请求参数
        var requestData = new
        {
            appid = config.AppId,
            mch_id = config.MerchantId,
            nonce_str = Guid.NewGuid().ToString("N"),
            body = subject,
            out_trade_no = orderId,
            total_fee = amount,
            spbill_create_ip = GetClientIpAddress(),
            notify_url = config.NotifyUrl,
            trade_type = "NATIVE"
        };

        // 生成签名
        var sign = GenerateWechatSign(requestData, config.WechatSecret);
        var requestWithSign = new Dictionary<string, object>
        {
            ["appid"] = requestData.appid,
            ["mch_id"] = requestData.mch_id,
            ["nonce_str"] = requestData.nonce_str,
            ["body"] = requestData.body,
            ["out_trade_no"] = requestData.out_trade_no,
            ["total_fee"] = requestData.total_fee,
            ["spbill_create_ip"] = requestData.spbill_create_ip,
            ["notify_url"] = requestData.notify_url,
            ["trade_type"] = requestData.trade_type,
            ["sign"] = sign
        };

        // 调用微信支付API
        var response = await CallWechatApiAsync(requestWithSign, config.Environment);
        
        if (response.ContainsKey("return_code") && response["return_code"] == "SUCCESS")
        {
            if (response.ContainsKey("result_code") && response["result_code"] == "SUCCESS")
            {
                var qrCode = response["code_url"];
                return new PaymentCreateOutput
                {
                    OrderId = orderId,
                    QrCode = qrCode,
                    Status = "pending",
                    ExpireTime = DateTime.Now.AddMinutes(30)
                };
            }
            else
            {
                var errMsg = response.ContainsKey("err_code_des") ? response["err_code_des"] : "未知错误";
                throw ResultOutput.Exception($"微信支付下单失败: {errMsg}");
            }
        }
        else
        {
            var returnMsg = response.ContainsKey("return_msg") ? response["return_msg"] : "未知错误";
            throw ResultOutput.Exception($"微信支付API调用失败: {returnMsg}");
        }
    }

    /// <summary>
    /// 验证支付宝回调签名
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<bool> VerifyAlipayCallbackAsync(Dictionary<string, string> parameters)
    {
        var config = await GetAlipayConfigAsync();
        if (config == null) return false;

        var sign = parameters.GetValueOrDefault("sign");
        if (string.IsNullOrEmpty(sign)) return false;

        // 移除sign参数
        parameters.Remove("sign");
        parameters.Remove("sign_type");

        // 生成签名
        var calculatedSign = GenerateAlipaySign(parameters, config.PrivateKey);
        return sign == calculatedSign;
    }

    /// <summary>
    /// 验证微信支付回调签名
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<bool> VerifyWechatCallbackAsync(Dictionary<string, string> parameters)
    {
        var config = await GetWechatConfigAsync();
        if (config == null) return false;

        var sign = parameters.GetValueOrDefault("sign");
        if (string.IsNullOrEmpty(sign)) return false;

        // 移除sign参数
        parameters.Remove("sign");

        // 生成签名
        var calculatedSign = GenerateWechatSign(parameters, config.WechatSecret);
        return sign == calculatedSign;
    }

    /// <summary>
    /// 查询支付宝订单状态
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public async Task<PaymentStatusOutput> QueryAlipayOrderAsync(string orderId)
    {
        var config = await GetAlipayConfigAsync();
        if (config == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["支付宝配置不存在"]);
        }

        var parameters = new Dictionary<string, string>
        {
            ["app_id"] = config.AppId,
            ["method"] = "alipay.trade.query",
            ["charset"] = "utf-8",
            ["sign_type"] = "RSA2",
            ["timestamp"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            ["version"] = "1.0",
            ["biz_content"] = JsonSerializer.Serialize(new { out_trade_no = orderId })
        };

        var sign = GenerateAlipaySign(parameters, config.PrivateKey);
        parameters["sign"] = sign;

        var response = await CallAlipayApiAsync(parameters, config.Environment);
        
        if (response.ContainsKey("alipay_trade_query_response"))
        {
            var queryResponse = JsonSerializer.Deserialize<JsonElement>(response["alipay_trade_query_response"]);
            
            if (queryResponse.TryGetProperty("code", out var code) && code.GetString() == "10000")
            {
                var tradeStatus = queryResponse.GetProperty("trade_status").GetString();
                var status = tradeStatus switch
                {
                    "TRADE_SUCCESS" => "paid",
                    "TRADE_CLOSED" => "failed",
                    "TRADE_FINISHED" => "paid",
                    _ => "pending"
                };

                return new PaymentStatusOutput
                {
                    OrderId = orderId,
                    Status = status,
                    Amount = (int)(queryResponse.GetProperty("total_amount").GetDouble() * 100),
                    TransactionId = queryResponse.TryGetProperty("trade_no", out var tradeNo) ? tradeNo.GetString() : null
                };
            }
        }

        return new PaymentStatusOutput
        {
            OrderId = orderId,
            Status = "pending"
        };
    }

    /// <summary>
    /// 查询微信支付订单状态
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public async Task<PaymentStatusOutput> QueryWechatOrderAsync(string orderId)
    {
        var config = await GetWechatConfigAsync();
        if (config == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["微信支付配置不存在"]);
        }

        var requestData = new
        {
            appid = config.AppId,
            mch_id = config.MerchantId,
            out_trade_no = orderId,
            nonce_str = Guid.NewGuid().ToString("N")
        };

        var sign = GenerateWechatSign(requestData, config.WechatSecret);
        var requestWithSign = new Dictionary<string, object>
        {
            ["appid"] = requestData.appid,
            ["mch_id"] = requestData.mch_id,
            ["out_trade_no"] = requestData.out_trade_no,
            ["nonce_str"] = requestData.nonce_str,
            ["sign"] = sign
        };

        var response = await CallWechatQueryApiAsync(requestWithSign, config.Environment);
        
        if (response.ContainsKey("return_code") && response["return_code"] == "SUCCESS")
        {
            var tradeState = response.GetValueOrDefault("trade_state");
            var status = tradeState switch
            {
                "SUCCESS" => "paid",
                "CLOSED" => "failed",
                "REVOKED" => "failed",
                _ => "pending"
            };

            return new PaymentStatusOutput
            {
                OrderId = orderId,
                Status = status,
                Amount = int.Parse(response.GetValueOrDefault("total_fee", "0")),
                TransactionId = response.GetValueOrDefault("transaction_id")
            };
        }

        return new PaymentStatusOutput
        {
            OrderId = orderId,
            Status = "pending"
        };
    }

    /// <summary>
    /// 获取支付宝配置
    /// </summary>
    /// <returns></returns>
    private async Task<PaymentConfigEntity?> GetAlipayConfigAsync()
    {
        return await _paymentConfigRep.Select
            .Where(a => a.PaymentMethod == "alipay" && a.Enabled)
            .FirstAsync();
    }

    /// <summary>
    /// 获取微信支付配置
    /// </summary>
    /// <returns></returns>
    private async Task<PaymentConfigEntity?> GetWechatConfigAsync()
    {
        return await _paymentConfigRep.Select
            .Where(a => a.PaymentMethod == "wechat" && a.Enabled)
            .FirstAsync();
    }

    /// <summary>
    /// 生成支付宝签名
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="privateKey"></param>
    /// <returns></returns>
    private string GenerateAlipaySign(Dictionary<string, string> parameters, string privateKey)
    {
        // 排序参数
        var sortedParams = parameters.OrderBy(x => x.Key).ToList();
        
        // 构建签名字符串
        var signString = string.Join("&", sortedParams.Select(x => $"{x.Key}={x.Value}"));
        
        // 使用RSA2签名
        using var rsa = RSA.Create();
        rsa.ImportFromPem(privateKey);
        
        var data = Encoding.UTF8.GetBytes(signString);
        var signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// 生成微信支付签名
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="secret"></param>
    /// <returns></returns>
    private string GenerateWechatSign(object parameters, string secret)
    {
        var dict = new Dictionary<string, object>();
        
        // 将对象转换为字典
        if (parameters is Dictionary<string, object> dictParams)
        {
            dict = dictParams;
        }
        else
        {
            var json = JsonSerializer.Serialize(parameters);
            dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
        }

        // 过滤空值和sign参数
        var filteredParams = dict
            .Where(x => x.Value != null && x.Key != "sign")
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Value?.ToString() ?? "");

        // 构建签名字符串
        var signString = string.Join("&", filteredParams.Select(x => $"{x.Key}={x.Value}")) + $"&key={secret}";
        
        // MD5签名
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(signString));
        return Convert.ToHexString(hash).ToUpper();
    }

    /// <summary>
    /// 调用支付宝API
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> CallAlipayApiAsync(Dictionary<string, string> parameters, string environment)
    {
        var url = environment == "production" 
            ? "https://openapi.alipay.com/gateway.do" 
            : "https://openapi.alipaydev.com/gateway.do";

        var content = string.Join("&", parameters.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));
        
        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));
        var responseContent = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent) ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// 调用微信支付API
    /// </summary>
    /// <param name="requestData"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> CallWechatApiAsync(Dictionary<string, object> requestData, string environment)
    {
        var url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        
        var xml = ConvertToXml(requestData);
        
        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsync(url, new StringContent(xml, Encoding.UTF8, "application/xml"));
        var responseContent = await response.Content.ReadAsStringAsync();
        
        return ParseXmlToDictionary(responseContent);
    }

    /// <summary>
    /// 调用微信支付查询API
    /// </summary>
    /// <param name="requestData"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, string>> CallWechatQueryApiAsync(Dictionary<string, object> requestData, string environment)
    {
        var url = "https://api.mch.weixin.qq.com/pay/orderquery";
        
        var xml = ConvertToXml(requestData);
        
        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsync(url, new StringContent(xml, Encoding.UTF8, "application/xml"));
        var responseContent = await response.Content.ReadAsStringAsync();
        
        return ParseXmlToDictionary(responseContent);
    }

    /// <summary>
    /// 转换为XML格式
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private string ConvertToXml(Dictionary<string, object> data)
    {
        var xml = new StringBuilder();
        xml.AppendLine("<xml>");
        
        foreach (var item in data)
        {
            xml.AppendLine($"<{item.Key}><![CDATA[{item.Value}]]></{item.Key}>");
        }
        
        xml.AppendLine("</xml>");
        return xml.ToString();
    }

    /// <summary>
    /// 解析XML为字典
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    private Dictionary<string, string> ParseXmlToDictionary(string xml)
    {
        var result = new Dictionary<string, string>();
        
        // 简单的XML解析，实际项目中建议使用专业的XML解析库
        var lines = xml.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (trimmedLine.StartsWith("<") && trimmedLine.EndsWith(">") && !trimmedLine.StartsWith("</"))
            {
                var startTag = trimmedLine.IndexOf('<') + 1;
                var endTag = trimmedLine.IndexOf('>');
                var tagName = trimmedLine.Substring(startTag, endTag - startTag);
                
                var startValue = trimmedLine.IndexOf("![CDATA[") + 8;
                var endValue = trimmedLine.LastIndexOf("]]>");
                if (startValue > 7 && endValue > startValue)
                {
                    var value = trimmedLine.Substring(startValue, endValue - startValue);
                    result[tagName] = value;
                }
            }
        }
        
        return result;
    }

    /// <summary>
    /// 获取客户端IP地址
    /// </summary>
    /// <returns></returns>
    private string GetClientIpAddress()
    {
        return IPHelper.GetIP(AppInfo.HttpContext?.Request);
    }
}
