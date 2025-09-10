using System.ComponentModel;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Core.Consts;

/// <summary>
/// �����
/// </summary>
[ScanCacheKeys]
public static partial class CacheKeys
{
    /// <summary>
    /// ��֤�� admin:captcha:guid
    /// </summary>
    [Description("��֤��")]
    public const string Captcha = "admin:captcha:";

    /// <summary>
    /// ������� admin:password:encrypt:guid
    /// </summary>
    [Description("�������")]
    public const string PassWordEncrypt = "admin:password:encrypt:";

    /// <summary>
    /// �û�Ȩ�� admin:user:permissions:userId
    /// </summary>
    [Description("�û�Ȩ��")]
    public const string UserPermission = "admin:user:permission:";

    /// <summary>
    /// ����Ȩ�� admin:user:data:permission:userId
    /// </summary>
    [Description("����Ȩ��")]
    public const string DataPermission = "admin:user:data:permission:";

    /// <summary>
    /// ������֤�� admin:sms:code:guid
    /// </summary>
    [Description("������֤��")]
    public const string SmsCode = "admin:sms:code:";

    /// <summary>
    /// ������֤�� admin:email:code:guid
    /// </summary>
    [Description("������֤��")]
    public const string EmailCode = "admin:email:code:";

    /// <summary>
    /// �ӿ��б� admin:api:list
    /// </summary>
    [Description("�ӿ��б�")]
    public const string ApiList = "admin:api:list";

    /// <summary>
    /// Excel�������ļ� admin:excel:error_mark:userId:fileId
    /// </summary>
    [Description("Excel�������ļ�")]
    public const string ExcelErrorMark = "admin:excel:error_mark:";

    /// <summary>
    /// ��ȡ������֤�뻺���
    /// </summary>
    /// <param name="mobile">�ֻ���</param>
    /// <param name="code">Ψһ��</param>
    /// <returns></returns>
    public static string GetSmsCodeKey(string mobile, string code) => $"{SmsCode}{mobile}:{code}";

    /// <summary>
    /// ��ȡ������֤�뻺���
    /// </summary>
    /// <param name="email">�ʼ���ַ</param>
    /// <param name="code">Ψһ��</param>
    /// <returns></returns>
    public static string GetEmailCodeKey(string email, string code) => $"{EmailCode}{email}:{code}";

    /// <summary>
    /// ��ȡ����Ȩ�޻����
    /// </summary>
    /// <param name="userId">�û�Id</param>
    /// <param name="apiPath">����ӿ�·��</param>
    /// <returns></returns>
    public static string GetDataPermissionKey(long userId, string apiPath = null)
    {
        if(apiPath.IsNull())
        {
            apiPath = AppInfo.CurrentDataPermissionApiPath;
        }

        return $"{DataPermission}{userId}{(apiPath.NotNull() ? (":" + apiPath) : "")}";
    }

    /// <summary>
    /// ��ȡ����Ȩ�޻����
    /// </summary>
    /// <param name="userId">�û�Id</param>
    /// <returns></returns>
    public static string GetUserPermissionKey(long userId) => $"{UserPermission}{userId}";

    /// <summary>
    /// ��ȡ����Ȩ��ģ��
    /// </summary>
    /// <param name="userId">�û�Id</param>
    /// <returns></returns>
    public static string GetDataPermissionPattern(long userId) => $"{DataPermission}{userId}*";

    /// <summary>
    /// ��ȡExcel�������ļ������
    /// </summary>
    /// <param name="userId">�û�Id</param>
    /// <param name="fileId">�ļ�Id</param>
    /// <returns></returns>
    public static string GetExcelErrorMarkKey(long userId, string fileId) => $"{ExcelErrorMark}{userId}{(fileId.NotNull() ? (":" + fileId) : "")}";
}
