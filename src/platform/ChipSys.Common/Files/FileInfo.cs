namespace ChipSys.Common.Files;

/// <summary>
/// �ļ���Ϣ
/// </summary>
public class FileInfo
{
    public FileInfo()
    {
    }

    /// <summary>
    /// ��ʼ���ļ���Ϣ
    /// </summary>
    /// <param name="fileName">�ļ�����</param>
    /// <param name="size">��С</param>
    public FileInfo(string fileName, long size = 0L)
    {
        FileName = fileName;
        Size = new FileSize(size);
        Extension = System.IO.Path.GetExtension(FileName)?.TrimStart('.');
    }

    /// <summary>
    /// �ϴ�·��
    /// </summary>
    public string UploadPath { get; set; }

    /// <summary>
    /// ����·��
    /// </summary>
    public string RequestPath { get; set; }

    /// <summary>
    /// ���·��
    /// </summary>
    public string RelativePath { get; set; }

    /// <summary>
    /// �ļ���
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    public string SaveName { get; set; }

    /// <summary>
    /// �ļ���С
    /// </summary>
    public FileSize Size { get; set; }

    /// <summary>
    /// ��չ��
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// �ļ�Ŀ¼
    /// </summary>
    public string FileDirectory => System.IO.Path.Combine(UploadPath, RelativePath).ToPath();

    /// <summary>
    /// �ļ�����·��
    /// </summary>
    public string FileRequestPath => System.IO.Path.Combine(RequestPath, RelativePath, SaveName).ToPath();

    /// <summary>
    /// �ļ����·��
    /// </summary>
    public string FileRelativePath => System.IO.Path.Combine(RelativePath, SaveName).ToPath();

    /// <summary>
    /// �ļ�·��
    /// </summary>
    public string FilePath => System.IO.Path.Combine(UploadPath, RelativePath, SaveName).ToPath();
}
