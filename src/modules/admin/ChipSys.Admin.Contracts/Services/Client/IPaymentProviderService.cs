using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 支付提供商服务接口
/// </summary>
public interface IPaymentProviderService
{
    /// <summary>
    /// 创建支付宝支付订单
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="amount"></param>
    /// <param name="subject"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    Task<PaymentCreateOutput> CreateAlipayOrderAsync(string orderId, int amount, string subject, string? description = null);

    /// <summary>
    /// 创建微信支付订单
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="amount"></param>
    /// <param name="subject"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    Task<PaymentCreateOutput> CreateWechatOrderAsync(string orderId, int amount, string subject, string? description = null);

    /// <summary>
    /// 验证支付宝回调签名
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<bool> VerifyAlipayCallbackAsync(Dictionary<string, string> parameters);

    /// <summary>
    /// 验证微信支付回调签名
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<bool> VerifyWechatCallbackAsync(Dictionary<string, string> parameters);

    /// <summary>
    /// 查询支付宝订单状态
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<PaymentStatusOutput> QueryAlipayOrderAsync(string orderId);

    /// <summary>
    /// 查询微信支付订单状态
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<PaymentStatusOutput> QueryWechatOrderAsync(string orderId);
}
