using ChipSys.Admin.Core.Dto;
using ChipSys.Client.Contracts.Services.Client.Dto;

namespace ChipSys.Client.Contracts.Services.Client;

/// <summary>
/// 支付服务接口
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// 创建支付订单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PaymentCreateOutput> CreatePaymentAsync(PaymentCreateInput input);

    /// <summary>
    /// 检查支付状态
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<PaymentStatusOutput> CheckPaymentStatusAsync(string orderId);

    /// <summary>
    /// 支付回调处理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> PaymentCallbackAsync(PaymentCallbackInput input);
}
