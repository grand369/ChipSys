using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Contracts.Domain.ChipMgr.Product
{
    /// <summary>
    /// ��Ʒ��Ӧ�̹�ϵ����������Ϣ��
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "product_supplier", OldName = DbConsts.ChipTableOldNamePrefix + "product_supplier")]
    public partial class ProductSupplierEntity : EntityBase
    {
        /// <summary>
        /// ��ƷId
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// ��Ӧ��Id
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// ���ּ۸�
        /// </summary>
        public decimal? PreviousPrice { get; set; }

        /// <summary>
        /// ��ǰ�۸�
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column(StringLength = 10)]
        public string Currency { get; set; } = "CNY";

        /// <summary>
        /// ��ɫ
        /// </summary>
        [Column(StringLength = 20)]
        public string? Condition { get; set; }

        /// <summary>
        /// ʹ������
        /// </summary>
        [Column(StringLength = 200)]
        public string? UsageDescription { get; set; }

        /// <summary>
        /// ��С����
        /// </summary>
        public int MOQ { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public int? LeadTimeDays { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public int StockQty { get; set; }

        /// <summary>
        /// ��Ч����
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// ��Ч��ֹ
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// ��Ʒ
        /// </summary>
        [NotGen]
        public ProductEntity Product { get; set; }

        /// <summary>
        /// ��Ӧ��
        /// </summary>
        [NotGen]
        public SupplierEntity Supplier { get; set; }
    }
}
