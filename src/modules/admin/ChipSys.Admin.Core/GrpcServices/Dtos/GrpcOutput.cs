using ProtoBuf;

namespace ChipSys.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// Grpc���
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.None)]
public class GrpcOutput<T>
{
    /// <summary>
    /// �Ƿ�ɹ����
    /// </summary>
    [ProtoMember(1)]
    public bool Success { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [ProtoMember(2)]
    public string Code { get; set; }

    /// <summary>
    /// ��Ϣ
    /// </summary>
    [ProtoMember(3)]
    public string Msg { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [ProtoMember(4)]
    public T Data { get; set; }
}
