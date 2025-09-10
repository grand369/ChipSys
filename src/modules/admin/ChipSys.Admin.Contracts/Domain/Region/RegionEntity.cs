using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;

namespace ChipSys.Admin.Domain.Region;

/// <summary>
/// ����
/// </summary>
[Table(Name = DbConsts.TableNamePrefix + "region")]
[Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Name), true)]
[Index("idx_{tablename}_02", nameof(ParentId) + "," + nameof(Code), true)]
public partial class RegionEntity : EntityBase, IChilds<RegionEntity>
{
    /// <summary>
    /// �ϼ�Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; }

    /// <summary>
    /// ���
    /// </summary>
    [Column(StringLength = 100)]
    public string ShortName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(MapType = typeof(int))]
    public RegionLevel Level { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 20)]
    public string Code { get; set; }

    /// <summary>
    /// ƴ��
    /// </summary>
    [Column(StringLength = 200)]
    public string Pinyin { get; set; }

    /// <summary>
    /// ƴ������ĸ
    /// </summary>
    [Column(StringLength = 20)]
    public string PinyinFirst { get; set; }

    /// <summary>
    /// ��������/����פ��
    /// </summary>
    [Column(StringLength = 100)]
    public string Capital { get; set; }

    /// <summary>
    /// �˿ڣ���λ�����ˣ�
    /// </summary>
    public int? Population { get; set; }

    /// <summary>
    /// �������λ��ƽ��ǧ�ף�
    /// </summary>
    public int? Area { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 20)]
    public string AreaCode { get; set; }

    /// <summary>
    /// �ʱ�
    /// </summary>
    [Column(StringLength = 20)]
    public string ZipCode { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public int? Sort { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public bool Hot { get; set; } = false;

    /// <summary>
    /// ����
    /// </summary>
    public bool Enabled { get; set; } = true;

    [Navigate(nameof(ParentId))]
    public List<RegionEntity> Childs { get; set; }
}