using ProtoBuf;

namespace ChipSys.Admin.Core.Protos;

/// <summary>
/// ProtoDecimal ��ʾ Grpc �������Ӧ�е� decimal
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class ProtoDecimal
{
    public decimal Value { get; set; }

    public ProtoDecimal() { }

    public ProtoDecimal(decimal value)
    {
        Value = value;
    }

    public static implicit operator ProtoDecimal(decimal value)
    {
        return new ProtoDecimal(value);
    }

    public static implicit operator decimal(ProtoDecimal result)
    {
        return result.Value;
    }
}
