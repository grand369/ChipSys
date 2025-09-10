using FreeSql.DataAnnotations;
using System.ComponentModel;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ʵ���Ա
/// </summary>
public class EntityMember<TKey> : Entity<TKey>, IMember, IDelete
{
    /// <summary>
    /// ��ԱId
    /// </summary>
    [Description("��ԱId")]
    [Column(Position = -23, CanUpdate = false)]
    public virtual long? MemberId { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    [Description("����ʱ��")]
    [Column(Position = -20, CanUpdate = false)]
    [ServerTime]
    public virtual DateTime? CreatedTime { get; set; }

    /// <summary>
    /// �޸�ʱ��
    /// </summary>
    [Description("�޸�ʱ��")]
    [Column(Position = -10)]
    [ServerTime(CanInsert = false, CanUpdate = true)]
    public virtual DateTime? ModifiedTime { get; set; }

    /// <summary>
    /// �Ƿ�ɾ��
    /// </summary>
    [Description("�Ƿ�ɾ��")]
    [Column(Position = -9)]
    public virtual bool IsDeleted { get; set; } = false;
}

/// <summary>
/// ʵ���Ա
/// </summary>
public class EntityMember : EntityMember<long>
{
}
