using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;

/// <summary>
/// 命中清单列表查询
/// </summary>
public class HitListGetListInput : QueryInput
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
