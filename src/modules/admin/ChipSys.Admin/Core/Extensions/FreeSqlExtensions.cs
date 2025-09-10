using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Domain.OperationLog;

namespace ChipSys.Admin.Core.Extensions;

/// <summary>
/// FreeSql��չ
/// </summary>
public static class FreeSqlExtensions
{
    /// <summary>
    /// ʹ����־���ݿ�
    /// </summary>
    /// <param name="that"></param>
    /// <param name="dbConfig"></param>
    public static void UseLogDb(this IFreeSql that, DbConfig dbConfig)
    {
        if (dbConfig.Key == DbKeys.LogDb)
        {
            if (dbConfig.Type != FreeSql.DataType.QuestDb)
            {
                that.CodeFirst.Entity<OperationLogEntity>(a =>
                {
                    a.HasIndex(a => new { a.ApiPath, a.CreatedTime }).HasName("idx_{tablename}_01");
                });
            }
        }
    }
}
