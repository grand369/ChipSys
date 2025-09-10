using ChipSys.Common.Extensions;

namespace ChipSys.Common.Files;

/// <summary>
/// �ļ���С
/// </summary>
public struct FileSize
{
    /// <summary>
    /// ��ʼ���ļ���С
    /// </summary>
    /// <param name="size">�ļ���С</param>
    /// <param name="unit">�ļ���С��λ</param>
    public FileSize(long size, FileSizeUnit unit = FileSizeUnit.Byte)
    {
        switch (unit)
        {
            case FileSizeUnit.K:
                Size = size * 1024; break;
            case FileSizeUnit.M:
                Size = size * 1024 * 1024; break;
            case FileSizeUnit.G:
                Size = size * 1024 * 1024 * 1024; break;
            default:
                Size = size; break;
        }
    }

    /// <summary>
    /// �ļ��ֽڳ���
    /// </summary>
    public long Size { get; }

    /// <summary>
    /// ��ȡ�ļ���С����λ���ֽ�
    /// </summary>
    public long GetSize()
    {
        return Size;
    }

    /// <summary>
    /// ��ȡ�ļ���С����λ��K
    /// </summary>
    public double GetSizeByK()
    {
        return (Size / 1024.0).ToDouble(2);
    }

    /// <summary>
    /// ��ȡ�ļ���С����λ��M
    /// </summary>
    public double GetSizeByM()
    {
        return (Size / 1024.0 / 1024.0).ToDouble(2);
    }

    /// <summary>
    /// ��ȡ�ļ���С����λ��G
    /// </summary>
    public double GetSizeByG()
    {
        return (Size / 1024.0 / 1024.0 / 1024.0).ToDouble(2);
    }

    /// <summary>
    /// �������
    /// </summary>
    public override string ToString()
    {
        if (Size >= 1024 * 1024 * 1024)
            return $"{GetSizeByG()} {FileSizeUnit.G.ToDescription()}";
        if (Size >= 1024 * 1024)
            return $"{GetSizeByM()} {FileSizeUnit.M.ToDescription()}";
        if (Size >= 1024)
            return $"{GetSizeByK()} {FileSizeUnit.K.ToDescription()}";
        return $"{Size} {FileSizeUnit.Byte.ToDescription()}";
    }
}
