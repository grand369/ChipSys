using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;

namespace ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier
{
    /// <summary>
    /// �ϴ��嵥��Ŀ
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "upload_list_item", OldName = DbConsts.ChipTableOldNamePrefix + "upload_list_item")]
    public partial class UploadListItemEntity : EntityTenantWithData
    {
        /// <summary>
        /// �嵥Id
        /// </summary>
        public long ListId { get; set; }

        /// <summary>
        /// ԭʼ����
        /// </summary>
        [Column(StringLength = 50)]
        public string? RawCode { get; set; }

        /// <summary>
        /// ԭʼƷ��
        /// </summary>
        [Column(StringLength = 50)]
        public string? RawBrand { get; set; }

        /// <summary>
        /// ԭʼ����
        /// </summary>
        [Column(StringLength = 200)]
        public string? RawDescription { get; set; }

        /// <summary>
        /// ƥ��״̬
        /// </summary>
        [Column(StringLength = 20)]
        public string MatchStatus { get; set; } = "Unmatched";

        /// <summary>
        /// ƥ�䵽�Ĳ�ƷId
        /// </summary>
        public long? MatchedProductId { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// �����嵥
        /// </summary>
        [NotGen]
        public UploadListEntity UploadList { get; set; }

        /// <summary>
        /// ƥ�䵽�Ĳ�Ʒ
        /// </summary>
        [NotGen]
        public ProductEntity? MatchedProduct { get; set; }
    }
}
