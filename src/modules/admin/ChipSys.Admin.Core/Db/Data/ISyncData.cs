using ChipSys.Admin.Core.Configs;

namespace ChipSys.Admin.Core.Db.Data;

/// <summary>
/// ͬ�����ݽӿ�
/// </summary>
public interface ISyncData
{
    Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null);
}
