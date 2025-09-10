using System.Reflection;

namespace ChipSys.Plugin.Lazy.SlideCaptcha.Core.Resources.Handler;

public class EmbeddedResourceHandler : IResourceHandler
{
    public const string TYPE = "embedded";

    public bool CanHandle(string handlerType)
    {
        return handlerType == TYPE;
    }

    public byte[] Handle(Resource resource)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream(resource.Data);
        return StreamToBytes(stream);
    }

    private static byte[] StreamToBytes(Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);

        // ���õ�ǰ����λ��Ϊ���Ŀ�ʼ 
        stream.Seek(0, SeekOrigin.Begin);
        return bytes;
    }
}
