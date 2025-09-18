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
using ChipSys.Client.Contracts.Domain.Client;
using FreeSql;
using System.Security.Cryptography;
using System.Text;

namespace ChipSys.Admin.Services.Client;

/// <summary>
/// 支付服务
/// </summary>
[Order(107)]
[DynamicApi(Area = "client")]
public class PaymentService : BaseService, IPaymentService, IDynamicApi
{
    private readonly AdminRepositoryBase<PaymentOrderEntity> _paymentOrderRep;
    private readonly AdminRepositoryBase<MemberLevelEntity> _memberLevelRep;
    private readonly IPaymentProviderService _paymentProviderService;
    private readonly AdminLocalizer _adminLocalizer;

    public PaymentService(
        AdminRepositoryBase<PaymentOrderEntity> paymentOrderRep,
        AdminRepositoryBase<MemberLevelEntity> memberLevelRep,
        IPaymentProviderService paymentProviderService,
        AdminLocalizer adminLocalizer)
    {
        _paymentOrderRep = paymentOrderRep;
        _memberLevelRep = memberLevelRep;
        _paymentProviderService = paymentProviderService;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// 创建支付订单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PaymentCreateOutput> CreatePaymentAsync(PaymentCreateInput input)
    {
        // 生成订单号
        var orderId = GenerateOrderId();
        
        // 创建支付订单
        var order = new PaymentOrderEntity
        {
            OrderId = orderId,
            MemberId = User.Id,
            Level = input.Level,
            Amount = input.Amount,
            PaymentMethod = input.PaymentMethod,
            Description = input.Description ?? $"升级到{input.Level}会员",
            Status = "pending",
            CreatedTime = DateTime.Now,
            ExpireTime = DateTime.Now.AddMinutes(30) // 30分钟过期
        };

        await _paymentOrderRep.InsertAsync(order);

        // 根据支付方式生成支付信息
        var result = new PaymentCreateOutput
        {
            OrderId = orderId,
            Status = "pending",
            ExpireTime = order.ExpireTime
        };

        if (input.PaymentMethod == "alipay")
        {
            // 调用支付宝API生成二维码
            var alipayResult = await _paymentProviderService.CreateAlipayOrderAsync(
                orderId, input.Amount, input.Description ?? "会员升级", input.Description);
            result.QrCode = alipayResult.QrCode;
        }
        else if (input.PaymentMethod == "wechat")
        {
            // 调用微信支付API生成二维码
            var wechatResult = await _paymentProviderService.CreateWechatOrderAsync(
                orderId, input.Amount, input.Description ?? "会员升级", input.Description);
            result.QrCode = wechatResult.QrCode;
        }

        return result;
    }

    /// <summary>
    /// 检查支付状态
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    public async Task<PaymentStatusOutput> CheckPaymentStatusAsync(string orderId)
    {
        var order = await _paymentOrderRep.Select
            .Where(a => a.OrderId == orderId && a.MemberId == User.Id)
            .FirstAsync();

        if (order == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["订单不存在"]);
        }

        return new PaymentStatusOutput
        {
            OrderId = order.OrderId,
            Status = order.Status,
            PaidTime = order.PaidTime,
            Amount = order.Amount,
            TransactionId = order.TransactionId
        };
    }

    /// <summary>
    /// 支付回调处理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> PaymentCallbackAsync(PaymentCallbackInput input)
    {
        var order = await _paymentOrderRep.Select
            .Where(a => a.OrderId == input.OrderId)
            .FirstAsync();

        if (order == null)
        {
            return false;
        }

        // 验证签名
        var parameters = new Dictionary<string, string>
        {
            ["orderId"] = input.OrderId,
            ["status"] = input.Status,
            ["transactionId"] = input.TransactionId ?? "",
            ["paidTime"] = input.PaidTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? ""
        };

        bool isValid = order.PaymentMethod switch
        {
            "alipay" => await _paymentProviderService.VerifyAlipayCallbackAsync(parameters),
            "wechat" => await _paymentProviderService.VerifyWechatCallbackAsync(parameters),
            _ => false
        };

        if (!isValid)
        {
            return false;
        }

        // 更新订单状态
        order.Status = input.Status;
        order.TransactionId = input.TransactionId;
        order.PaidTime = input.PaidTime;

        await _paymentOrderRep.UpdateAsync(order);

        // 如果支付成功，升级会员等级
        if (input.Status == "paid")
        {
            await UpgradeMemberLevel(order.MemberId, order.Level);
        }

        return true;
    }

    /// <summary>
    /// 升级会员等级
    /// </summary>
    /// <param name="memberId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private async Task UpgradeMemberLevel(long memberId, string level)
    {
        // 获取等级模板
        var levelTemplate = await _memberLevelRep.Select
            .Where(a => a.Level == level && a.MemberId == 0 && a.Enabled)
            .FirstAsync();

        if (levelTemplate == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["等级模板不存在"]);
        }

        // 创建新的会员等级记录
        var memberLevel = new MemberLevelEntity
        {
            MemberId = memberId,
            Level = level,
            CategoryLimit = levelTemplate.CategoryLimit,
            ProductDataLimit = levelTemplate.ProductDataLimit,
            SupplierDataLimit = levelTemplate.SupplierDataLimit,
            ShowFullContactInfo = levelTemplate.ShowFullContactInfo,
            Enabled = true,
            EffectiveTime = DateTime.Now,
            ExpireTime = DateTime.Now.AddYears(1), // 1年有效期
            CreatedTime = DateTime.Now
        };

        await _memberLevelRep.InsertAsync(memberLevel);
    }

    /// <summary>
    /// 生成订单号
    /// </summary>
    /// <returns></returns>
    private string GenerateOrderId()
    {
        return $"PAY{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
    }

}
