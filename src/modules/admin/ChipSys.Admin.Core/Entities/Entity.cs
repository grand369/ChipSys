using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using ChipSys.Admin.Core.Attributes;

namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ʵ��ӿ�
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntity<TKey>
{
    /// <summary>
    /// ����Id
    /// </summary>
    TKey Id { get; set; }
}

/// <summary>
/// ʵ��ӿ�
/// </summary>
public interface IEntity : IEntity<long>
{
}

/// <summary>
/// ʵ��
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class Entity<TKey> : IEntity<TKey>
{
    /// <summary>
    /// ����Id
    /// </summary>
    [Description("����Id")]
    [Snowflake]
    [Column(Position = 1, IsIdentity = false, IsPrimary = true)]
    [JsonPropertyOrder(-30)]
    public virtual TKey Id { get; set; }
}

/// <summary>
/// ʵ��
/// </summary>
public class Entity : Entity<long>
{
}
