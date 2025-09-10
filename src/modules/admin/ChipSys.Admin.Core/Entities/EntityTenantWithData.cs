using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ʵ���⻧����Ȩ��
/// </summary>
public class EntityTenantWithData<TKey> : EntityTenant<TKey>, IData where TKey : struct
{
    /// <summary>
    /// ӵ����Id
    /// </summary>
    [Description("ӵ����Id")]
    [Column(Position = -42)]
    public virtual long? OwnerId { get; set; }

    /// <summary>
    /// ӵ���߲���Id
    /// </summary>
    [Description("ӵ���߲���Id")]
    [Column(Position = -41)]
    public virtual long? OwnerOrgId { get; set; }

    /// <summary>
    /// ӵ���߲�������
    /// </summary>
    [Description("ӵ���߲�������")]
    [Column(Position = -40)]
    public virtual string? OwnerOrgName { get; set; }
}

/// <summary>
/// ʵ���⻧����Ȩ��
/// </summary>
public class EntityTenantWithData : EntityTenantWithData<long>
{
}
