using FreeScheduler;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Core.Repositories;

namespace ChipSys.Admin.Repositories;

public class TaskLogRepository : RepositoryBase<TaskLog>, ITaskLogRepository
{
    public TaskLogRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.TaskDb, uowm)
    {
    }
}
