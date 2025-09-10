using FreeScheduler;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Core.Repositories;

namespace ChipSys.Admin.Repositories;

public class TaskRepository : RepositoryBase<TaskInfo>, ITaskRepository
{
    public TaskRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.TaskDb, uowm)
    {
    }
}
