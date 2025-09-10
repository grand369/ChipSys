using System.Text;

namespace ChipSys.Common.Helpers;

public class FileHelper : IDisposable
{
    private bool _alreadyDispose = false;

    public FileHelper()
    {
    }

    ~FileHelper()
    {
        Dispose();
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (_alreadyDispose) return;
        _alreadyDispose = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #region д�ļ�

    /// <summary>
    /// д�ļ�
    /// </summary>
    /// <param name="Path">�ļ�·��</param>
    /// <param name="Strings">�ļ�����</param>
    public static void WriteFile(string Path, string Strings)
    {
        if (!File.Exists(Path))
        {
            File.Create(Path).Close();
        }
        var streamWriter = new StreamWriter(Path, false);
        streamWriter.Write(Strings);
        streamWriter.Close();
        streamWriter.Dispose();
    }

    /// <summary>
    /// д�ļ�
    /// </summary>
    /// <param name="Path">�ļ�·��</param>
    /// <param name="Strings">�ļ�����</param>
    /// <param name="encode">�����ʽ</param>
    public static void WriteFile(string Path, string Strings, Encoding encode)
    {
        if (!File.Exists(Path))
        {
            File.Create(Path).Close();
        }
        var streamWriter = new StreamWriter(Path, false, encode);
        streamWriter.Write(Strings);
        streamWriter.Close();
        streamWriter.Dispose();
    }

    #endregion д�ļ�

    #region ���ļ�

    /// <summary>
    /// ���ļ�
    /// </summary>
    /// <param name="Path">�ļ�·��</param>
    /// <returns></returns>
    public static string ReadFile(string Path)
    {
        string s;
        if (!File.Exists(Path))
            s = "��������Ӧ��Ŀ¼";
        else
        {
            var streamReader = new StreamReader(Path);
            s = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();
        }

        return s;
    }

    /// <summary>
    /// ���ļ�
    /// </summary>
    /// <param name="Path">�ļ�·��</param>
    /// <param name="encode">�����ʽ</param>
    /// <returns></returns>
    public static string ReadFile(string Path, Encoding encode)
    {
        string s;
        if (!File.Exists(Path))
            s = "��������Ӧ��Ŀ¼";
        else
        {
            var streamReader = new StreamReader(Path, encode);
            s = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();
        }

        return s;
    }

    #endregion ���ļ�
}
