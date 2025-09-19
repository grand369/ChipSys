using ChipSys.Admin.Core.Consts;

namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// 启用权限库事务
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class AdminTransactionAttribute : TransactionAttribute
{
    public AdminTransactionAttribute():base(DbKeys.AdminDb)
    {
    }
}
