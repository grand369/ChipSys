using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 用户清单列表查询
/// </summary>
public class UserListGetListInput : QueryInput
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
