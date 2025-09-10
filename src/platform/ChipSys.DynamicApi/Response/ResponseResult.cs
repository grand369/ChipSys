namespace ChipSys.DynamicApi;

public class ResponseResult<T>
{
    /// <summary>
    /// �Ƿ�ɹ����
    /// </summary>
    public bool Success { get; private set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ��Ϣ
    /// </summary>
    public string Msg { get; private set; }

    /// <summary>
    /// ����
    /// </summary>
    public T Data { get; private set; }
}
