using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ʵ���Ա�⻧
/// </summary>
public class EntityMemberWithTenant<TKey> : EntityMember<TKey>, ITenant where TKey : struct
{
    /// <summary>
    /// �⻧Id
    /// </summary>
    [Description("�⻧Id")]
    [Column(Position = 2, CanUpdate = false)]
    [JsonPropertyOrder(-20)]
    public virtual long? TenantId { get; set; }
}

/// <summary>
/// ʵ���Ա�⻧
/// </summary>
public class EntityMemberWithTenant : EntityMemberWithTenant<long>
{
}
