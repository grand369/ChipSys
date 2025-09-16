using ChipSys.Common.Converters;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace ChipSys.Common.Helpers;

/// <summary>
/// Json������
/// </summary>
public class JsonHelper
{
    // �̰߳�ȫ��������
    private static readonly Lock _configLock = new();
    private static readonly JsonSerializerOptions _jsonSerializerOptions = CreateDefaultOptions();

    /// <summary>
    /// ����Ĭ�ϵ�JSON���л�ѡ��
    /// </summary>
    private static JsonSerializerOptions CreateDefaultOptions()
    {
        return new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
            //��Сд������
            PropertyNameCaseInsensitive = true,
            //�շ���������
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //����JSONע��	
            ReadCommentHandling = JsonCommentHandling.Skip,
            //����β�涺��
            AllowTrailingCommas = true,
            //�����л������ŵ�����
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            //����ѭ������
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            //�����ַ�ת��
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            //��ʽ�����
            WriteIndented = true,
            //�Զ���ת����
            Converters =
            {
                new DateTimeConverter(),
                // ����ʱ��ת����
                new NullableDateTimeConverter(),
                // ����ö��ת������֧���ַ������������ַ�ʽ
                new FlexibleEnumConverter(),
            },
        };
    }

    /// <summary>
    /// ��ȡ��ǰJSON���л�ѡ��ĸ���
    /// </summary>
    public static JsonSerializerOptions GetCurrentOptions()
    {
        using (_configLock.EnterScope())
        {
            return new JsonSerializerOptions(_jsonSerializerOptions);
        }
    }

    /// <summary>
    /// ����Ĭ�ϵ�JSON���л�ѡ��
    /// </summary>
    /// <param name="configure">����ί��</param>
    public static void ConfigureOptions(Action<JsonSerializerOptions> configure)
    {
        using (_configLock.EnterScope())
        {
            configure?.Invoke(_jsonSerializerOptions);
        }
    }

    /// <summary>
    /// ���л�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj,  _jsonSerializerOptions);
    }

    /// <summary>
    /// ���л�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string Serialize<T>(T obj, JsonSerializerOptions options)
    {
        return JsonSerializer.Serialize(obj, options ?? _jsonSerializerOptions);
    }

    /// <summary>
    /// �����л�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
    }

    /// <summary>
    /// �����л�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static T Deserialize<T>(string json, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<T>(json, options ?? _jsonSerializerOptions);
    }

    /// <summary>
    /// �����л�
    /// </summary>
    /// <param name="json">json�ı�</param>
    /// <param name="type">����</param>
    /// <returns></returns>
    public static object Deserialize(string json, Type type)
    {
        return JsonSerializer.Deserialize(json, type, _jsonSerializerOptions);
    }

    /// <summary>
    /// �����л�
    /// </summary>
    /// <param name="json">json�ı�</param>
    /// <param name="type">����</param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static object Deserialize(string json, Type type, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize(json, type, options ?? _jsonSerializerOptions);
    }
}
