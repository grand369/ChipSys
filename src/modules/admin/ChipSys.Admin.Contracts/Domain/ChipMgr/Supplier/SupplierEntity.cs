using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using System;
using System.Collections.Generic;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;

namespace ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;

/// <summary>
/// ��Ӧ��
/// </summary>
[Table(Name = DbConsts.ChipTableNamePrefix + "supplier", OldName = DbConsts.ChipTableOldNamePrefix + "supplier")]
public partial class SupplierEntity : EntityTenantWithData
{
    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    [Column(StringLength = 100)]
    public string? CompanyName { get; set; }
    /// <summary>
    /// 编码
    /// </summary>
    [Column(StringLength = 50)]
    public string? Code { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    [Column(StringLength = 50)]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// 联系人姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string? ContactName { get; set; }

    /// <summary>
    /// 联系人电话
    /// </summary>
    [Column(StringLength = 20)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// 联系人邮箱
    /// </summary>
    [Column(StringLength = 100)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 经营范围
    /// </summary>
    [Column(StringLength = 500)]
    public string? BusinessScope { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    [Column(StringLength = 20)]
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Column(StringLength = 100)]
    public string? Email { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [Column(StringLength = 200)]
    public string? Address { get; set; }

    /// <summary>
    /// 评分
    /// </summary>
    public int Rating { get; set; }
    [Column(StringLength = 100)]
    public string? Website { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; }
    /// <summary>
    ///  描述
    /// </summary>
    [Column(StringLength = 500)]
    public string? Description { get; set; }

    /// <summary>
    /// 联系人列表
    /// </summary>
    [NotGen]
    public ICollection<ContactEntity> Contacts { get; set; } = new List<ContactEntity>();

    /// <summary>
    /// 产品供应商关系列表
    /// </summary>
    [NotGen]
    public ICollection<ProductSupplierEntity> ProductSuppliers { get; set; } = new List<ProductSupplierEntity>();
}
