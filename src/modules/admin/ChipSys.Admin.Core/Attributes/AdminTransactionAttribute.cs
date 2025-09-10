using ChipSys.Admin.Core.Consts;

namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// ����Ȩ�޿�����
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class AdminTransactionAttribute : TransactionAttribute
{
    public AdminTransactionAttribute():base(DbKeys.AdminDb)
    {
    }
}
