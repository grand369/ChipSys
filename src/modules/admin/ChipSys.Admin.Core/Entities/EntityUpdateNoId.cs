using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ʵ���޸�������Id
/// </summary>
public class EntityUpdateNoId : EntityAddNoId, IEntityUpdate
{
    /// <summary>
    /// �޸����û�Id
    /// </summary>
    [Description("�޸����û�Id")]
    [Column(Position = -13)]
    [JsonPropertyOrder(10000)]
    public virtual long? ModifiedUserId { get; set; }

    /// <summary>
    /// �޸����û���
    /// </summary>
    [Description("�޸����û���")]
    [Column(Position = -12), MaxLength(60)]
    [JsonPropertyOrder(10001)]
    public virtual string ModifiedUserName { get; set; }

    /// <summary>
    /// �޸�������
    /// </summary>
    [Description("�޸�������")]
    [Column(Position = -11), MaxLength(60)]
    [JsonPropertyOrder(10001)]
    public virtual string ModifiedUserRealName { get; set; }

    /// <summary>
    /// �޸�ʱ��
    /// </summary>
    [Description("�޸�ʱ��")]
    [JsonPropertyOrder(10002)]
    [Column(Position = -10)]
    [ServerTime(CanInsert = false, CanUpdate = true)]
    public virtual DateTime? ModifiedTime { get; set; }
}
