using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Domain.User;

namespace ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier
{
    /// <summary>
    /// 用户上传清单
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "upload_list", OldName = DbConsts.ChipTableOldNamePrefix + "upload_list")]
    public partial class UploadListEntity : EntityBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [Column(StringLength = 200)]
        public string? FileName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column(StringLength = 20)]
        public string Status { get; set; } = "Pending";

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
        /// 清单条目列表
        /// </summary>
        [NotGen]
        public ICollection<UploadListItemEntity> Items { get; set; } = new List<UploadListItemEntity>();
    }
}
