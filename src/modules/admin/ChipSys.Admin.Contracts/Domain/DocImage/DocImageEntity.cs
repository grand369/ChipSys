using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;
using ChipSys.Admin.Domain.Doc;

namespace ChipSys.Admin.Domain.DocImage;

/// <summary>
/// �ĵ�ͼƬ
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "doc_image", OldName = DbConsts.TableOldNamePrefix + "document_image")]
[Index("idx_{tablename}_01", nameof(DocumentId) + "," + nameof(Url), true)]
public class DocImageEntity : EntityAdd
{
    /// <summary>
    /// �ĵ�Id
    /// </summary>
    public long DocumentId { get; set; }

    public DocEntity Document { get; set; }

    /// <summary>
    /// ����·��
    /// </summary>
    public string Url { get; set; }
}
