using System.ComponentModel;

namespace ChipSys.Admin.Core.Consts;

/// <summary>
/// 订阅名
/// </summary>
public class SubscribeNames
{
    /// <summary>
    /// 短信单发
    /// </summary>
    [Description("短信单发")]
    public const string SmsSingleSend = "ChipSys.Admin.sms:singleSend";

    /// <summary>
    /// 短信验证码发送
    /// </summary>
    [Description("短信验证码发送")]
    public const string SmsSendCode = "ChipSys.Admin.sms:sendCode";

    /// <summary>
    /// 邮件单发
    /// </summary>
    [Description("邮件单发")]
    public const string EmailSingleSend = "ChipSys.Admin.email:singleSend";

    /// <summary>
    /// 邮箱验证码发送
    /// </summary>
    [Description("邮箱验证码发送")]
    public const string EmailSendCode = "ChipSys.Admin.email:sendCode";

    /// <summary>
    /// 用户部门转移
    /// </summary>
    [Description("用户部门转移")]
    public const string UserOrgChange = "ChipSys.Admin.user.orgChange";

    /// <summary>
    /// 登录日志添加
    /// </summary>
    [Description("登录日志添加")]
    public const string LoginLogAdd = "ChipSys.Admin.loginLog.add";

    /// <summary>
    /// 操作日志添加
    /// </summary>
    [Description("操作日志添加")]
    public const string OperationLogAdd = "ChipSys.Admin.operationLog.add";
}
