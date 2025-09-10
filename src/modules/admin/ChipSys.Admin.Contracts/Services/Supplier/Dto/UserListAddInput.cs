using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 用户清单添加
/// </summary>
public class UserListAddInput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [Required(ErrorMessage = "请选择用户")]
    public long UserId { get; set; }

    /// <summary>
    /// 产品供应商Id
    /// </summary>
    [Required(ErrorMessage = "请选择产品供应商")]
    public long ProductSupplierId { get; set; }

    /// <summary>
    /// 清单类型: Hit/Favorite/Inquiry/Buy
    /// </summary>
    [Required(ErrorMessage = "请选择清单类型")]
    [StringLength(20, ErrorMessage = "清单类型长度不能超过20个字符")]
    public string ListType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [StringLength(20, ErrorMessage = "状态长度不能超过20个字符")]
    public string Status { get; set; } = "Active";

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
