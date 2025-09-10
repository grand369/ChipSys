using ProtoBuf;

namespace ChipSys.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// �ӿ�
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.None)]
public class ApiGrpcOutput
{
    /// <summary>
    /// ����Id
    /// </summary>
    [ProtoMember(1)]
    public long Id { get; set; }

    /// <summary>
    /// ����ģ��
    /// </summary>
    [ProtoMember(2)]
    public long ParentId { get; set; }

    /// <summary>
    /// �ӿ�����
    /// </summary>
    [ProtoMember(3)]
    public string Label { get; set; }

    /// <summary>
    /// �ӿڵ�ַ
    /// </summary>
    [ProtoMember(4)]
    public string Path { get; set; }

    /// <summary>
    /// ���ýӿ���־
    /// </summary>
    [ProtoMember(5)]
    public bool EnabledLog { get; set; }

    /// <summary>
    /// �����������
    /// </summary>
    [ProtoMember(6)]
    public bool EnabledParams { get; set; }

    /// <summary>
    /// ������Ӧ���
    /// </summary>
    [ProtoMember(7)]
    public bool EnabledResult { get; set; }
}
