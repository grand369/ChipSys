using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core.Converters;

/// <summary>
/// ����ö��ת������֧���ַ������������ַ�ʽ
/// </summary>
public class FlexibleEnumConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
    {
        return (JsonConverter)Activator.CreateInstance(typeof(EnumConverter<>).MakeGenericType(type));
    }

    private class EnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        // ��ȡö�ٵĻ�������
        private readonly Type _underlyingType = Enum.GetUnderlyingType(typeof(T));

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonTokenType tokenType = reader.TokenType;

            // �����ַ������͵�ֵ
            if (tokenType == JsonTokenType.String)
            {
                string enumString = reader.GetString();
                if (Enum.TryParse(enumString, true, out T value))
                    return value;
            }
            // �����������͵�ֵ
            else if (tokenType == JsonTokenType.Number)
            {
                try
                {
                    // ���ݻ������Ͷ�̬��������ֵ
                    object numericValue = _underlyingType.Name switch
                    {
                        nameof(Int32) => reader.GetInt32(),
                        nameof(UInt32) => reader.GetUInt32(),
                        nameof(Int64) => reader.GetInt64(),
                        nameof(UInt64) => reader.GetUInt64(),
                        nameof(Int16) => (short)reader.GetInt32(),
                        nameof(UInt16) => (ushort)reader.GetUInt32(),
                        nameof(Byte) => (byte)reader.GetUInt32(),
                        nameof(SByte) => (sbyte)reader.GetInt32(),
                        _ => throw new JsonException($"Unsupported underlying type: {_underlyingType}")
                    };

                    return (T)Enum.ToObject(typeof(T), numericValue);
                }
                catch
                {
                    // ����ת��ʧ��ʱ�����ַ�������
                    string stringValue = reader.TryGetInt64(out long longVal)
                        ? longVal.ToString()
                        : reader.GetDouble().ToString();

                    if (Enum.TryParse(stringValue, true, out T value))
                        return value;
                }
            }

            throw new JsonException($"Unable to convert value to {typeof(T).Name}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // ֱ��д������ֵ������ʵ�ʻ������ͣ�
            switch (Convert.GetTypeCode(Enum.ToObject(typeof(T), value)))
            {
                case TypeCode.Byte:
                    writer.WriteNumberValue(Convert.ToByte(value));
                    break;
                case TypeCode.SByte:
                    writer.WriteNumberValue(Convert.ToSByte(value));
                    break;
                case TypeCode.Int16:
                    writer.WriteNumberValue(Convert.ToInt16(value));
                    break;
                case TypeCode.UInt16:
                    writer.WriteNumberValue(Convert.ToUInt16(value));
                    break;
                case TypeCode.Int32:
                    writer.WriteNumberValue(Convert.ToInt32(value));
                    break;
                case TypeCode.UInt32:
                    writer.WriteNumberValue(Convert.ToUInt32(value));
                    break;
                case TypeCode.Int64:
                    writer.WriteNumberValue(Convert.ToInt64(value));
                    break;
                case TypeCode.UInt64:
                    writer.WriteNumberValue(Convert.ToUInt64(value));
                    break;
                default:
                    writer.WriteStringValue(value.ToString());
                    break;
            }
        }
    }
}
