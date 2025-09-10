using ChipSys.Admin.Core.Entities;
using FreeSql.DataAnnotations;

namespace ChipSys.Admin.Domain;

/// <summary>
/// ��־
/// </summary>
public abstract class LogAbstract : EntityAdd, ITenant
{
    /// <summary>
    /// �⻧Id
    /// </summary>
    [Column(Position = 2, CanUpdate = false)]
    public long? TenantId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 60)]
    public string Name { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    [Column(StringLength = 100)]
    public string IP { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 100)]
    public string Country { get; set; }

    /// <summary>
    /// ʡ��
    /// </summary>
    [Column(StringLength = 100)]
    public string Province { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Column(StringLength = 100)]
    public string City { get; set; }

    /// <summary>
    /// ���������
    /// </summary>
    [Column(StringLength = 100)]
    public string Isp { get; set; }

    /// <summary>
    /// �����
    /// </summary>
    [Column(StringLength = 100)]
    public string Browser { get; set; }

    /// <summary>
    /// ����ϵͳ
    /// </summary>
    [Column(StringLength = 100)]
    public string Os { get; set; }

    /// <summary>
    /// �豸
    /// </summary>
    [Column(StringLength = 50)]
    public string Device { get; set; }

    /// <summary>
    /// �������Ϣ
    /// </summary>
    [Column(StringLength = -1)]
    public string BrowserInfo { get; set; }

    /// <summary>
    /// ��ʱ�����룩
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// ����״̬
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    [Column(StringLength = -1)]
    public string Msg { get; set; }
}
