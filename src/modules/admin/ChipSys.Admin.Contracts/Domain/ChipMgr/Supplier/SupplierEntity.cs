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
public partial class SupplierEntity : EntityBase
{
    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; }

    /// <summary>
    /// ��ϵ��
    /// </summary>
    [Column(StringLength = 50)]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// �绰
    /// </summary>
    [Column(StringLength = 20)]
    public string? Phone { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 100)]
    public string? Email { get; set; }

    /// <summary>
    /// ��ַ
    /// </summary>
    [Column(StringLength = 200)]
    public string? Address { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// ��ϵ���б�
    /// </summary>
    [NotGen]
    public ICollection<ContactEntity> Contacts { get; set; } = new List<ContactEntity>();

    /// <summary>
    /// ��Ʒ��Ӧ�̹�ϵ�б�
    /// </summary>
    [NotGen]
    public ICollection<ProductSupplierEntity> ProductSuppliers { get; set; } = new List<ProductSupplierEntity>();
}
