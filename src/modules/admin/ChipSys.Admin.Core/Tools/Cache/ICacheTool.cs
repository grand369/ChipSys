namespace ChipSys.Admin.Tools.Cache;

/// <summary>
/// ����ӿ�
/// </summary>
public interface ICacheTool
{
    /// <summary>
    /// ������ key ����ʱɾ�� key
    /// </summary>
    /// <param name="key">��</param>
    long Del(params string[] key);

    /// <summary>
    /// ������ key ����ʱɾ�� key
    /// </summary>
    /// <param name="key">��</param>
    /// <returns></returns>
    Task<long> DelAsync(params string[] key);

    /// <summary>
    /// ������ key ģ�����ʱɾ��
    /// </summary>
    /// <param name="pattern">keyģ��</param>
    /// <returns></returns>
    Task<long> DelByPatternAsync(string pattern);

    /// <summary>
    /// ������ key �Ƿ����
    /// </summary>
    /// <param name="key">��</param>
    /// <returns></returns>
    bool Exists(string key);

    /// <summary>
    /// ������ key �Ƿ����
    /// </summary>
    /// <param name="key">��</param>
    /// <returns></returns>
    Task<bool> ExistsAsync(string key);

    /// <summary>
    /// ��ȡָ�� key ��ֵ
    /// </summary>
    /// <param name="key">��</param>
    /// <returns></returns>
    string Get(string key);

    /// <summary>
    /// ��ȡָ�� key ��ֵ
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="key">��</param>
    /// <returns></returns>
    T Get<T>(string key);

    /// <summary>
    /// ��ȡָ�� key ��ֵ
    /// </summary>
    /// <param name="key">��</param>
    /// <returns></returns>
    Task<string> GetAsync(string key);

    /// <summary>
    /// ��ȡָ�� key ��ֵ
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="key">��</param>
    /// <returns></returns>
    Task<T> GetAsync<T>(string key);

    /// <summary>
    /// ����ָ�� key ��ֵ������д�����object��֧��string | byte[] | ��ֵ | ����
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="value">ֵ</param>
    void Set(string key, object value);

    /// <summary>
    /// ����ָ�� key ��ֵ������д�����object��֧��string | byte[] | ��ֵ | ����
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="value">ֵ</param>
    /// <param name="expire">��Ч��</param>
    void Set(string key, object value, TimeSpan expire);

    /// <summary>
    /// ����ָ�� key ��ֵ������д�����object��֧��string | byte[] | ��ֵ | ����
    /// </summary>
    /// <param name="key">��</param>
    /// <param name="value">ֵ</param>
    /// <param name="expire">��Ч��</param>
    /// <returns></returns>
    Task SetAsync(string key, object value, TimeSpan? expire = null);

    /// <summary>
    /// ��ȡ�����û���
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="key">��</param>
    /// <param name="func">��ȡ���ݵķ���</param>
    /// <param name="expire">��Ч��</param>
    /// <returns></returns>
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> func, TimeSpan? expire = null);

    /// <summary>
    /// ���л����
    /// </summary>
    List<string> Keys { get; }

    /// <summary>
    /// ���� key ģ�������л����
    /// </summary>
    List<string> GetKeysByPattern(string pattern);
}
