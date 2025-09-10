using ChipSys.Admin.Core.Configs;

namespace ChipSys.Admin.Core.Db.Data;

/// <summary>
/// 生成数据接口
/// </summary>
public interface IGenerateData
{
    Task GenerateDataAsync(IFreeSql db, AppConfig appConfig);
}
