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
    /// �����嵥
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "hit_list", OldName = DbConsts.ChipTableOldNamePrefix + "hit_list")]
    public partial class HitListEntity : EntityTenantWithData
    {
        /// <summary>
        /// �û�Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// �嵥��ĿId
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// ��Ʒ��Ӧ��Id
        /// </summary>
        public long ProductSupplierId { get; set; }

        /// <summary>
        /// ���Ŷ�
        /// </summary>
        public decimal? Confidence { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// �û�
        /// </summary>
        [NotGen]
        public UserEntity User { get; set; }

        /// <summary>
        /// �嵥��Ŀ
        /// </summary>
        [NotGen]
        public UploadListItemEntity Item { get; set; }

        /// <summary>
        /// ��Ʒ��Ӧ��
        /// </summary>
        [NotGen]
        public ProductSupplierEntity ProductSupplier { get; set; }
    }

}
