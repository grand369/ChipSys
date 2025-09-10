using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 命中清单分页查询
/// </summary>
public class HitListGetPageInput : PageInput<HitListGetPageInput>
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// 清单项目Id
    /// </summary>
    public long? ItemId { get; set; }

    /// <summary>
    /// 产品供应商Id
    /// </summary>
    public long? ProductSupplierId { get; set; }
}
