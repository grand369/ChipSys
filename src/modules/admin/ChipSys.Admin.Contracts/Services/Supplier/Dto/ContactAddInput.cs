using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Supplier.Dto;

/// <summary>
/// 联系人添加
/// </summary>
public class ContactAddInput
{
    /// <summary>
    /// 供应商Id
    /// </summary>
    [Required(ErrorMessage = "请选择供应商")]
    public long SupplierId { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Required(ErrorMessage = "请输入联系人姓名")]
    [StringLength(50, ErrorMessage = "联系人姓名长度不能超过50个字符")]
    public string Name { get; set; }

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
    /// 职位
    /// </summary>
    [StringLength(50, ErrorMessage = "职位长度不能超过50个字符")]
    public string? Position { get; set; }
}
