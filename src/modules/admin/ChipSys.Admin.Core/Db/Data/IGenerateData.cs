using ChipSys.Admin.Core.Configs;

namespace ChipSys.Admin.Core.Db.Data;

/// <summary>
/// �������ݽӿ�
/// </summary>
public interface IGenerateData
{
    Task GenerateDataAsync(IFreeSql db, AppConfig appConfig);
}
