using Microsoft.Extensions.Configuration;

namespace ChipSys.Common.Helpers;

/// <summary>
/// ���ð�����
/// </summary>
public class ConfigHelper
{
    /* ʹ���ȸ���
    var uploadConfig = new ConfigHelper().Load("uploadconfig", _env.EnvironmentName, true);
    services.Configure<UploadConfig>(uploadConfig);

    private readonly UploadConfig _uploadConfig;
    public ImgController(IOptionsMonitor<UploadConfig> uploadConfig)
    {
        _uploadConfig = uploadConfig.CurrentValue;
    }
    */

    /// <summary>
    /// ���������ļ�
    /// </summary>
    /// <param name="fileName">�ļ�����</param>
    /// <param name="environmentName">��������</param>
    /// <param name="optional">��ѡ</param>
    /// <param name="reloadOnChange">�Զ�����</param>
    /// <returns></returns>
    public static IConfiguration Load(string fileName, string environmentName = "", bool optional = true, bool reloadOnChange = false)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Configs");
        if (!Directory.Exists(filePath))
            return null;

        var builder = new ConfigurationBuilder()
            .SetBasePath(filePath)
            .AddJsonFile(fileName + ".json", optional, reloadOnChange);

        if (environmentName.NotNull())
        {
            builder.AddJsonFile(fileName + "." + environmentName + ".json", optional: optional, reloadOnChange: reloadOnChange);
        }

        return builder.Build();
    }

    /// <summary>
    /// ���������Ϣ
    /// </summary>
    /// <typeparam name="T">������Ϣ</typeparam>
    /// <param name="fileName">�ļ�����</param>
    /// <param name="environmentName">��������</param>
    /// <param name="optional">��ѡ</param>
    /// <param name="reloadOnChange">�Զ�����</param>
    /// <returns></returns>
    public static T Get<T>(string fileName, string environmentName = "", bool optional = true, bool reloadOnChange = false)
    {
        var configuration = Load(fileName, environmentName, optional, reloadOnChange);
        if (configuration == null)
            return default;

        return configuration.Get<T>();
    }

    /// <summary>
    /// ��ʵ��������Ϣ
    /// </summary>
    /// <param name="fileName">�ļ�����</param>
    /// <param name="instance">ʵ������</param>
    /// <param name="environmentName">��������</param>
    /// <param name="optional">��ѡ</param>
    /// <param name="reloadOnChange">�Զ�����</param>
    public static void Bind(string fileName, object instance, string environmentName = "", bool optional = true, bool reloadOnChange = false)
    {
        var configuration = Load(fileName, environmentName, optional, reloadOnChange);
        if (configuration == null || instance == null)
            return;

        configuration.Bind(instance);
    }
}
