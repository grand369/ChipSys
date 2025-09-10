using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Core.Repositories;
using ChipSys.Admin.Domain;

namespace ChipSys.Admin.Repositories;

public class TaskExtRepository : RepositoryBase<TaskInfoExt>, ITaskExtRepository
{
    public TaskExtRepository(UnitOfWorkManagerCloud uowm) : base(DbKeys.TaskDb, uowm)
    {
    }
}
