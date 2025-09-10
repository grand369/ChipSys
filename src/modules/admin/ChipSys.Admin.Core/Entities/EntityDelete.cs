using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ʵ��ɾ��
/// </summary>
public class EntityDelete<TKey> : EntityUpdate<TKey>, IDelete where TKey : struct
{
    /// <summary>
    /// �Ƿ�ɾ��
    /// </summary>
    [Description("�Ƿ�ɾ��")]
    [Column(Position = -9)]
    public virtual bool IsDeleted { get; set; } = false;
}

/// <summary>
/// ʵ��ɾ��
/// </summary>
public class EntityDelete : EntityDelete<long>
{
}
