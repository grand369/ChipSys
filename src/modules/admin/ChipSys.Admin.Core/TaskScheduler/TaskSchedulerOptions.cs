using FreeSql;
using ChipSys.Admin.Core.Consts;

namespace ChipSys.Admin.Tools.TaskScheduler;

/// <summary>
/// TaskScheduler����
/// </summary>
public class TaskSchedulerOptions
{
    /// <summary>
    /// ���ݿ��
    /// </summary>
    public string DbKey { get; set; } = DbKeys.AdminDb;

    /// <summary>
    /// ���ݿ�ʵ��
    /// </summary>
    public IFreeSql FreeSql { get; set; }

    /// <summary>
    /// ���ʵ��
    /// </summary>
    public FreeSqlCloud FreeSqlCloud { get; set; }

    /// <summary>
    /// ����FreeSql
    /// </summary>
    public Action<IFreeSql> ConfigureFreeSql { get; set; }

    /// <summary>
    /// ����FreeSql
    /// </summary>
    public Action<FreeSchedulerBuilder> ConfigureFreeSchedulerBuilder { get; set; }
}
