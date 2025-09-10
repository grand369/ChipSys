using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ʵ�崴��
/// </summary>
public class EntityAdd<TKey> : Entity<TKey>, IEntityAdd where TKey : struct
{
    /// <summary>
    /// �������û�Id
    /// </summary>
    [Description("�������û�Id")]
    [Column(Position = -23, CanUpdate = false)]
    public virtual long? CreatedUserId { get; set; }

    /// <summary>
    /// �������û���
    /// </summary>
    [Description("�������û���")]
    [Column(Position = -22, CanUpdate = false), MaxLength(60)]
    public virtual string CreatedUserName { get; set; }

    /// <summary>
    /// ����������
    /// </summary>
    [Description("����������")]
    [Column(Position = -21, CanUpdate = false), MaxLength(60)]
    public virtual string CreatedUserRealName { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    [Description("����ʱ��")]
    [Column(Position = -20, CanUpdate = false)]
    [ServerTime]
    public virtual DateTime? CreatedTime { get; set; }
}

/// <summary>
/// ʵ�崴��
/// </summary>
public class EntityAdd : EntityAdd<long>
{
}
