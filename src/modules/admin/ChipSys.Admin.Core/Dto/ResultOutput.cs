using System.Net;
using ChipSys.Admin.Core.Exceptions;

namespace ChipSys.Admin.Core.Dto;

/// <summary>
/// ������
/// </summary>
public class ResultOutput<T> : IResultOutput<T>
{
    /// <summary>
    /// �Ƿ�ɹ����
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// ��Ϣ
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// �ɹ�
    /// </summary>
    /// <param name="data">����</param>
    /// <param name="msg">��Ϣ</param>
    public ResultOutput<T> Ok(T data, string msg = null)
    {
        Success = true;
        Data = data;
        Msg = msg;

        return this;
    }

    /// <summary>
    /// ʧ��
    /// </summary>
    /// <param name="msg">��Ϣ</param>
    /// <param name="data">����</param>
    /// <returns></returns>
    public ResultOutput<T> NotOk(string msg = null, T data = default)
    {
        Success = false;
        Msg = msg;
        Data = data;

        return this;
    }
}

/// <summary>
/// ��̬������
/// </summary>
public static partial class ResultOutput
{
    /// <summary>
    /// �ɹ�
    /// </summary>
    /// <param name="data">����</param>
    /// <param name="msg">��Ϣ</param>
    /// <returns></returns>
    public static IResultOutput<T> Ok<T>(T data = default(T), string msg = null)
    {
        return new ResultOutput<T>().Ok(data, msg);
    }

    /// <summary>
    /// �ɹ�
    /// </summary>
    /// <returns></returns>
    public static IResultOutput<string> Ok()
    {
        return Ok<string>();
    }

    /// <summary>
    /// ʧ��
    /// </summary>
    /// <param name="msg">��Ϣ</param>
    /// <param name="data">����</param>
    /// <returns></returns>
    public static IResultOutput<T> NotOk<T>(string msg = null, T data = default)
    {
        return new ResultOutput<T>().NotOk(msg, data);
    }

    /// <summary>
    /// ʧ��
    /// </summary>
    /// <param name="msg">��Ϣ</param>
    /// <returns></returns>
    public static IResultOutput<string> NotOk(string msg = null)
    {
        return new ResultOutput<string>().NotOk(msg);
    }

    /// <summary>
    /// ϵͳ�쳣
    /// </summary>
    /// <param name="msg">��Ϣ</param>
    /// <param name="code">����</param>
    /// <param name="statusCode">״̬����</param>
    /// <returns></returns>
    public static AppException Exception(string msg = null, string code = null, int statusCode = (int)HttpStatusCode.OK)
    {
        return new AppException(msg, code, statusCode);
    }

    /// <summary>
    /// ���ݲ���ֵ���ؽ��
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static IResultOutput<T> Result<T>(bool success)
    {
        return success ? Ok<T>() : NotOk<T>();
    }

    /// <summary>
    /// ���ݲ���ֵ���ؽ��
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static IResultOutput<string> Result(bool success)
    {
        return success ? Ok() : NotOk();
    }
}
