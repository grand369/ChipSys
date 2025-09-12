using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.User;

namespace ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier
{
    /// <summary>
    /// 命中清单
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "hit_list", OldName = DbConsts.ChipTableOldNamePrefix + "hit_list")]
    public partial class HitListEntity : EntityTenantWithData
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 清单条目Id
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// 产品供应商Id
        /// </summary>
        public long ProductSupplierId { get; set; }

        /// <summary>
        /// 置信度
        /// </summary>
        public decimal? Confidence { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [NotGen]
        public UserEntity User { get; set; }

        /// <summary>
        /// 清单条目
        /// </summary>
        [NotGen]
        public UploadListItemEntity Item { get; set; }

        /// <summary>
        /// 产品供应商
        /// </summary>
        [NotGen]
        public ProductSupplierEntity ProductSupplier { get; set; }
    }

}
