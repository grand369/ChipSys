using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 供应商修改
/// </summary>
public class SupplierUpdateInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required(ErrorMessage = "请选择要修改的供应商")]
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Required(ErrorMessage = "请输入供应商名称")]
    [StringLength(100, ErrorMessage = "供应商名称长度不能超过100个字符")]
    public string Name { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    [StringLength(50, ErrorMessage = "联系人长度不能超过50个字符")]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [StringLength(20, ErrorMessage = "电话长度不能超过20个字符")]
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
    public string? Email { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [StringLength(200, ErrorMessage = "地址长度不能超过200个字符")]
    public string? Address { get; set; }

    /// <summary>
    /// 评级
    /// </summary>
    public int Rating { get; set; }
}
