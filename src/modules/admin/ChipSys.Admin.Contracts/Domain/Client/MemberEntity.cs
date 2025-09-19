using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Client.Contracts.Domain.Client;

/// <summary>
/// 会员实体
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "client_member", OldName = DbConsts.ChipTableOldNamePrefix + "client_member")]
public partial class MemberEntity : EntityTenantWithData
{
    /// <summary>
    /// 关联的用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 会员昵称
    /// </summary>
    [Column(StringLength = 50)]
    public string? NickName { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string? RealName { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    [Column(StringLength = 18)]
    public string? IdCard { get; set; }

    /// <summary>
    /// 性别：Male-男，Female-女，Other-其他
    /// </summary>
    [Column(StringLength = 10)]
    public string? Gender { get; set; }

    /// <summary>
    /// 所属省份
    /// </summary>
    [Column(StringLength = 50)]
    public string? Province { get; set; }

    /// <summary>
    /// 所属城市
    /// </summary>
    [Column(StringLength = 50)]
    public string? City { get; set; }

    /// <summary>
    /// 所属区县
    /// </summary>
    [Column(StringLength = 50)]
    public string? District { get; set; }

    /// <summary>
    /// 详细地址
    /// </summary>
    [Column(StringLength = 200)]
    public string? Address { get; set; }

    /// <summary>
    /// 微信OpenId
    /// </summary>
    [Column(StringLength = 100)]
    public string? WechatOpenId { get; set; }

    /// <summary>
    /// 微信UnionId
    /// </summary>
    [Column(StringLength = 100)]
    public string? WechatUnionId { get; set; }

    /// <summary>
    /// 微信昵称
    /// </summary>
    [Column(StringLength = 100)]
    public string? WechatNickName { get; set; }

    /// <summary>
    /// 微信头像
    /// </summary>
    [Column(StringLength = 500)]
    public string? WechatAvatar { get; set; }

    /// <summary>
    /// 是否已完善信息
    /// </summary>
    public bool IsProfileComplete { get; set; } = false;

    /// <summary>
    /// 会员等级：Free-免费会员，Basic-基础版，Standard-标准版，Premium-高级版，Enterprise-企业版
    /// </summary>
    [Column(StringLength = 20)]
    public string Level { get; set; } = "Free";

    /// <summary>
    /// 等级生效时间
    /// </summary>
    public DateTime? LevelEffectiveTime { get; set; }

    /// <summary>
    /// 等级过期时间
    /// </summary>
    public DateTime? LevelExpireTime { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;
}
