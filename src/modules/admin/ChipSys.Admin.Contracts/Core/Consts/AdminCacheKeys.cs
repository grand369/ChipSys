using System.ComponentModel;

namespace ChipSys.Admin.Contracts.Core.Consts;

/// <summary>
/// �����
/// </summary>
public static partial class AdminCacheKeys
{
    /// <summary>
    /// ���� admin:org:tenantId
    /// </summary>
    [Description("����")]
    public const string Org = "admin:org:";

    /// <summary>
    /// ��ȡ���Ż����
    /// </summary>
    /// <param name="tenantId">�⻧Id</param>
    /// <returns></returns>
    public static string GetOrgKey(long? tenantId) => $"{Org}{tenantId}";
}
