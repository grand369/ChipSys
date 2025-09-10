using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChipSys.Common.Converters;

/// <summary>
/// ����ʱ��ת����
/// </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                // ����Ĭ��ʱ�䣨�� DateTime.MinValue������������׳���ȷ�쳣
                return default;
                // �����׳��쳣��throw new JsonException("�޷������ַ���ת��Ϊ DateTime��");
            }
            if (DateTime.TryParse(stringValue, out var dateTime))
            {
                return dateTime;
            }
        }

        // �����������ַ������ͣ������֣�
        try
        {
            return reader.GetDateTime();
        }
        catch (FormatException ex)
        {
            throw new JsonException("��Ч�� DateTime ��ʽ��", ex);
        }
    }
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFFK"));
    }
}

/// <summary>
/// �ɿ�����ʱ��ת����
/// </summary>
public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();

            // ������ַ����� null �ַ���
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            // ���Խ�������ʱ��
            if (DateTime.TryParse(stringValue, out var dateTime))
            {
                return dateTime;
            }

            // ����޷��������׳���ȷ���쳣
            throw new JsonException($"�޷���ֵ '{stringValue}' ת��Ϊ DateTime��");
        }

        // �����������ͣ�������ʱ�����
        try
        {
            return reader.GetDateTime();
        }
        catch (Exception ex)
        {
            throw new JsonException("��Ч�� DateTime ��ʽ��", ex);
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFFK"));
        }
    }
}
