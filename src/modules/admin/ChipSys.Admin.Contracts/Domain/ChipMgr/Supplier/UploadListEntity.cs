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
    /// �û��ϴ��嵥
    /// </summary>
    [Table(Name = DbConsts.ChipTableNamePrefix + "upload_list", OldName = DbConsts.ChipTableOldNamePrefix + "upload_list")]
    public partial class UploadListEntity : EntityBase
    {
        /// <summary>
        /// �û�Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// �ļ���
        /// </summary>
        [Column(StringLength = 200)]
        public string? FileName { get; set; }

        /// <summary>
        /// ״̬
        /// </summary>
        [Column(StringLength = 20)]
        public string Status { get; set; } = "Pending";

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
        /// �嵥��Ŀ�б�
        /// </summary>
        [NotGen]
        public ICollection<UploadListItemEntity> Items { get; set; } = new List<UploadListItemEntity>();
    }
}
