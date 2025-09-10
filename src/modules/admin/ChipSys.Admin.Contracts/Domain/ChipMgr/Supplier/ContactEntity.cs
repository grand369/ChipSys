using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier
{
    /// <summary>
    /// 联系人
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "contact", OldName = DbConsts.ChipTableOldNamePrefix + "contact")]
    public partial class ContactEntity : EntityBase
    {
        /// <summary>
        /// 供应商Id
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

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
        /// 职位
        /// </summary>
        [Column(StringLength = 50)]
        public string? Position { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [NotGen]
        public SupplierEntity Supplier { get; set; }
    }

}
