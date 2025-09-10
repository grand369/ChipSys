namespace ChipSys.Admin.Core.Dto;

/// <summary>
/// �������ӿ�
/// </summary>
public interface IResultOutput
{
    /// <summary>
    /// �Ƿ�ɹ�
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// ��Ϣ
    /// </summary>
    string Msg { get; }

    /// <summary>
    /// ����
    /// </summary>
    string Code { get; set; }
}

/// <summary>
/// ���ͽ������ӿ�
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResultOutput<T> : IResultOutput
{
    /// <summary>
    /// ��������
    /// </summary>
    T Data { get; }
}
