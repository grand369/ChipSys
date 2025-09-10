using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;

namespace ChipSys.Admin.Domain.OperationLog;

/// <summary>
/// ������־
/// </summary>
[Database(DbNames.Log)]
[Table(Name = DbConsts.TableNamePrefix + "operation_log")]
public partial class OperationLogEntity : LogAbstract
{
    /// <summary>
    /// �ӿ�����
    /// </summary>
    [Column(Position = 2, StringLength = 50)]
    public string ApiLabel { get; set; }

    /// <summary>
    /// �ӿڵ�ַ
    /// </summary>
    [Column(Position = 3, StringLength = 500)]
    public string ApiPath { get; set; }

    /// <summary>
    /// �ӿڷ���
    /// </summary>
    [Column(Position = 4, StringLength = 50)]
    public string ApiMethod { get; set; }

    /// <summary>
    /// �������
    /// </summary>
    [Column(StringLength = -1)]
    public string Params { get; set; }

    /// <summary>
    /// ״̬��
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// ��Ӧ���
    /// </summary>
    [Column(StringLength = -1)]
    public string Result { get; set; }
}
