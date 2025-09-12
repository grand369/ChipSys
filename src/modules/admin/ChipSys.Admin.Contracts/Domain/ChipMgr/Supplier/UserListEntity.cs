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
    /// �û������嵥����ע/ѯ��/�ɹ���
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "user_list", OldName = DbConsts.ChipTableOldNamePrefix + "user_list")]
    public partial class UserListEntity : EntityTenantWithData
    {
        /// <summary>
        /// �û�Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// ��Ʒ��Ӧ��Id
        /// </summary>
        public long ProductSupplierId { get; set; }

        /// <summary>
        /// �嵥����: Hit/Favorite/Inquiry/Buy
        /// </summary>
        [Column(StringLength = 20)]
        public string ListType { get; set; }

        /// <summary>
        /// ״̬
        /// </summary>
        [Column(StringLength = 20)]
        public string Status { get; set; } = "Active";

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
        /// ��Ʒ��Ӧ��
        /// </summary>
        [NotGen]
        public ProductSupplierEntity ProductSupplier { get; set; }
    }
}
