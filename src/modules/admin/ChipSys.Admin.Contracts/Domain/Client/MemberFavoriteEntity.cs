using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 会员收藏实体
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "client_member_favorite", OldName = DbConsts.ChipTableOldNamePrefix + "client_member_favorite")]
public partial class MemberFavoriteEntity : EntityTenantWithData
{
    /// <summary>
    /// 会员用户Id
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// 收藏类型：Supplier-供应商，Product-产品，Contact-联系人
    /// </summary>
    [Column(StringLength = 20)]
    public string FavoriteType { get; set; }

    /// <summary>
    /// 收藏对象Id
    /// </summary>
    public long FavoriteId { get; set; }

    /// <summary>
    /// 收藏对象名称
    /// </summary>
    [Column(StringLength = 200)]
    public string? FavoriteName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(StringLength = 500)]
    public string? Remark { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}
