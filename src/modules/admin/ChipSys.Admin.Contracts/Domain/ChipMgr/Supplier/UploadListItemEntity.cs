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
    /// 上传清单条目
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "upload_list_item", OldName = DbConsts.ChipTableOldNamePrefix + "upload_list_item")]
    public partial class UploadListItemEntity : EntityTenantWithData
    {
        /// <summary>
        /// 清单Id
        /// </summary>
        public long ListId { get; set; }

        /// <summary>
        /// 原始编码
        /// </summary>
        [Column(StringLength = 50)]
        public string? RawCode { get; set; }

        /// <summary>
        /// 原始品牌
        /// </summary>
        [Column(StringLength = 50)]
        public string? RawBrand { get; set; }

        /// <summary>
        /// 原始描述
        /// </summary>
        [Column(StringLength = 200)]
        public string? RawDescription { get; set; }

        /// <summary>
        /// 匹配状态
        /// </summary>
        [Column(StringLength = 20)]
        public string MatchStatus { get; set; } = "Unmatched";

        /// <summary>
        /// 匹配到的产品Id
        /// </summary>
        public long? MatchedProductId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 所属清单
        /// </summary>
        [NotGen]
        public UploadListEntity UploadList { get; set; }

        /// <summary>
        /// 匹配到的产品
        /// </summary>
        [NotGen]
        public ProductEntity? MatchedProduct { get; set; }
    }
}
