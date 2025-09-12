using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;

/// <summary>
/// 用户清单分页查询
/// </summary>
public class UserListGetPageInput : PageInput<UserListGetPageInput>
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// 产品供应商Id
    /// </summary>
    public long? ProductSupplierId { get; set; }

    /// <summary>
    /// 清单类型
    /// </summary>
    public string ListType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; }
}
