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
    /// ��ϵ��
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "contact", OldName = DbConsts.ChipTableOldNamePrefix + "contact")]
    public partial class ContactEntity : EntityBase
    {
        /// <summary>
        /// ��Ӧ��Id
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

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
        /// ְλ
        /// </summary>
        [Column(StringLength = 50)]
        public string? Position { get; set; }

        /// <summary>
        /// ��Ӧ��
        /// </summary>
        [NotGen]
        public SupplierEntity Supplier { get; set; }
    }

}
