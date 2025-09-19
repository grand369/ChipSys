using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 会员等级实体
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "client_member_level", OldName = DbConsts.ChipTableOldNamePrefix + "client_member_level")]
public partial class MemberLevelEntity : EntityTenantWithData
{
    /// <summary>
    /// 会员用户Id（0表示等级模板）
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// 等级名称：Free-免费会员，Basic-基础版，Standard-标准版，Premium-高级版，Enterprise-企业版
    /// </summary>
    [Column(StringLength = 20)]
    public string Level { get; set; } = "Free";

    /// <summary>
    /// 等级显示名称
    /// </summary>
    [Column(StringLength = 50)]
    public string LevelName { get; set; } = "免费会员";

    /// <summary>
    /// 可查看的产品分类数量限制
    /// </summary>
    public int CategoryLimit { get; set; } = 0;

    /// <summary>
    /// 产品数据查看限制（条数）
    /// </summary>
    public int ProductDataLimit { get; set; } = 50;

    /// <summary>
    /// 供应商数据查看限制（条数）
    /// </summary>
    public int SupplierDataLimit { get; set; } = 50;

    /// <summary>
    /// 是否显示完整联系人信息
    /// </summary>
    public bool ShowFullContactInfo { get; set; } = false;

    /// <summary>
    /// 等级原价（元）
    /// </summary>
    public decimal OriginalPrice { get; set; } = 0;

    /// <summary>
    /// 等级折扣后价格（元）
    /// </summary>
    public decimal DiscountedPrice { get; set; } = 0;

    /// <summary>
    /// 折扣百分比（0-100）
    /// </summary>
    public decimal DiscountPercent { get; set; } = 0;

    /// <summary>
    /// 等级生效时间
    /// </summary>
    public DateTime? EffectiveTime { get; set; }

    /// <summary>
    /// 等级过期时间
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}
