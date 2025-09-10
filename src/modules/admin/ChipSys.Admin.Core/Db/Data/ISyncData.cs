using ChipSys.Admin.Core.Configs;

namespace ChipSys.Admin.Core.Db.Data;

/// <summary>
/// 同步数据接口
/// </summary>
public interface ISyncData
{
    Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null);
}
