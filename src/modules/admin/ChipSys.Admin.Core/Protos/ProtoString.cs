using ProtoBuf;

namespace ChipSys.Admin.Core.Protos;

/// <summary>
/// ProtoString ��ʾ Grpc �������Ӧ�е� string
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ProtoString
{
    public string Value { get; set; }

    public ProtoString() { }

    public ProtoString(string value)
    {
        Value = value;
    }

    public static implicit operator ProtoString(string value)
    {
        return new ProtoString(value);
    }

    public static implicit operator string(ProtoString result)
    {
        return result.Value;
    }
}
