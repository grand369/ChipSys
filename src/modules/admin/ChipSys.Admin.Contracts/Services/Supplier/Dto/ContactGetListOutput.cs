namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 联系人列表查询输出
/// </summary>
public class ContactGetListOutput : ContactGetOutput
{
    /// <summary>
    /// 供应商名称
    /// </summary>
    public string SupplierName { get; set; }
}
