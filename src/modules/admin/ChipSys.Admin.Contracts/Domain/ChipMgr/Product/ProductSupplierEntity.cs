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
    /// 产品供应商关系（包含价格信息等）
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "product_supplier", OldName = DbConsts.ChipTableOldNamePrefix + "product_supplier")]
    public partial class ProductSupplierEntity : EntityTenantWithData
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 之前价格
        /// </summary>
        public decimal? PreviousPrice { get; set; }

        /// <summary>
        /// 当前价格
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        [Column(StringLength = 10)]
        public string Currency { get; set; } = "CNY";

        /// <summary>
        /// 成色
        /// </summary>
        [Column(StringLength = 20)]
        public string? Condition { get; set; }

        /// <summary>
        /// 使用描述
        /// </summary>
        [Column(StringLength = 200)]
        public string? UsageDescription { get; set; }

        /// <summary>
        /// 供应商型号
        /// </summary>
        [Column(StringLength = 200)]
        public string SupplierModel { get; set; }

        /// <summary>
        /// 最小起订量
        /// </summary>
        public int MOQ { get; set; }

        /// <summary>
        /// 交期天数
        /// </summary>
        public int? LeadTimeDays { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockQty { get; set; }

        /// <summary>
        /// 有效开始
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// 有效截止
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        [NotGen]
        public ProductEntity Product { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [NotGen]
        public SupplierEntity Supplier { get; set; }
    }
}