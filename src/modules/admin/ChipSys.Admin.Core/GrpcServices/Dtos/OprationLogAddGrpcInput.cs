using ProtoBuf;

namespace ChipSys.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// ������־
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.None)]
public class OperationLogAddGrpcInput
{
    /// <summary>
    /// ����
    /// </summary>
    [ProtoMember(1)]
    public string Name { get; set; }

    /// <summary>
    /// �ӿ�����
    /// </summary>
    [ProtoMember(2)]
    public string ApiLabel { get; set; }

    /// <summary>
    /// �ӿڵ�ַ
    /// </summary>
    [ProtoMember(3)]
    public string ApiPath { get; set; }

    /// <summary>
    /// �ӿ��ύ����
    /// </summary>
    [ProtoMember(4)]
    public string ApiMethod { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    [ProtoMember(5)]
    public string IP { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [ProtoMember(6)]
    public string Country { get; set; }

    /// <summary>
    /// ʡ��
    /// </summary>
    [ProtoMember(7)]
    public string Province { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [ProtoMember(8)]
    public string City { get; set; }

    /// <summary>
    /// ���������
    /// </summary>
    [ProtoMember(9)]
    public string Isp { get; init; }

    /// <summary>
    /// �����
    /// </summary>
    [ProtoMember(10)]
    public string Browser { get; set; }

    /// <summary>
    /// ����ϵͳ
    /// </summary>
    [ProtoMember(11)]
    public string Os { get; set; }

    /// <summary>
    /// �豸
    /// </summary>
    [ProtoMember(12)]
    public string Device { get; set; }

    /// <summary>
    /// �������Ϣ
    /// </summary>
    [ProtoMember(13)]
    public string BrowserInfo { get; set; }

    /// <summary>
    /// ��ʱ�����룩
    /// </summary>
    [ProtoMember(14)]
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// ����״̬
    /// </summary>
    [ProtoMember(15)]
    public bool? Status { get; set; }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    [ProtoMember(16)]
    public string Msg { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    [ProtoMember(17)]
    public string Params { get; set; }

    /// <summary>
    /// ״̬��
    /// </summary>
    [ProtoMember(18)]
    public int? StatusCode { get; set; }

    /// <summary>
    /// �������
    /// </summary>
    [ProtoMember(19)]
    public string Result { get; set; }

    /// <summary>
    /// �⻧Id
    /// </summary>
    [ProtoMember(20)]
    public long? TenantId { get; set; }

    /// <summary>
    /// �������û�Id
    /// </summary>
    [ProtoMember(21)]
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// �������û���
    /// </summary>
    [ProtoMember(22)]
    public string CreatedUserName { get; set; }

    /// <summary>
    /// ����������
    /// </summary>
    [ProtoMember(23)]
    public string CreatedUserRealName { get; set; }
}
