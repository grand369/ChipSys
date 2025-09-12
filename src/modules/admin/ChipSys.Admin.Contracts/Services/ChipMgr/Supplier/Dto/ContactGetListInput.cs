using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Contracts.Services.ChipMgr.Supplier.Dto;

/// <summary>
/// 联系人列表查询
/// </summary>
public class ContactGetListInput : QueryInput
{
    /// <summary>
    /// 供应商Id
    /// </summary>
    public long? SupplierId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }
}
