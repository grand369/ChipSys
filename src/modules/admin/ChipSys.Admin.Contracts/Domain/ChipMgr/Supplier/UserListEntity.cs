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
    /// 用户操作清单（关注/询价/采购）
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "user_list", OldName = DbConsts.ChipTableOldNamePrefix + "user_list")]
    public partial class UserListEntity : EntityTenantWithData
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 产品供应商Id
        /// </summary>
        public long ProductSupplierId { get; set; }

        /// <summary>
        /// 清单类型: Hit/Favorite/Inquiry/Buy
        /// </summary>
        [Column(StringLength = 20)]
        public string ListType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column(StringLength = 20)]
        public string Status { get; set; } = "Active";

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
        /// 产品供应商
        /// </summary>
        [NotGen]
        public ProductSupplierEntity ProductSupplier { get; set; }
    }
}
