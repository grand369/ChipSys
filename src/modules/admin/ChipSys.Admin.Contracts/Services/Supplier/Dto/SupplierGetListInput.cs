using ChipSys.Admin.Core.Dto;

namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 供应商列表查询
/// </summary>
public class SupplierGetListInput : QueryInput
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    public string ContactPerson { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 评级
    /// </summary>
    public int? Rating { get; set; }
}
