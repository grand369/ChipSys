using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;

/// <summary>
/// 命中清单修改
/// </summary>
public class HitListUpdateInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "请选择要修改的命中清单")]
    public long Id { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    [Required(ErrorMessage = "请选择用户")]
    public long UserId { get; set; }

    /// <summary>
    /// 清单项目Id
    /// </summary>
    [Required(ErrorMessage = "请选择清单项目")]
    public long ItemId { get; set; }

    /// <summary>
    /// 产品供应商Id
    /// </summary>
    [Required(ErrorMessage = "请选择产品供应商")]
    public long ProductSupplierId { get; set; }

    /// <summary>
    /// 置信度
    /// </summary>
    public decimal? Confidence { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
