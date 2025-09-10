using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ʵ��汾
/// </summary>
public class EntityVersion<TKey> : EntityBase<TKey>, IVersion where TKey : struct
{
    /// <summary>
    /// �汾
    /// </summary>
    [Description("�汾")]
    [Column(Position = -30, IsVersion = true)]
    public virtual long Version { get; set; }
}

/// <summary>
/// ʵ��汾
/// </summary>
public class EntityVersion : EntityVersion<long>
{
}
