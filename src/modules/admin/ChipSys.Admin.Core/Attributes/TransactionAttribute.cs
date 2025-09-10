using FreeSql;
using System.Data;

namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// ��������
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class TransactionAttribute : Attribute
{
    /// <summary>
    /// ���񴫲���ʽ
    /// </summary>
    public Propagation Propagation { get; set; } = Propagation.Required;

    /// <summary>
    /// ������뼶��
    /// </summary>
    public IsolationLevel IsolationLevel { get; set; }

    /// <summary>
    /// ���ݿ�ע���
    /// </summary>
    public string DbKey { get; set; }

    public TransactionAttribute()
    {
    }

    public TransactionAttribute(string dbKey)
    {
        DbKey = dbKey;
    }
}
