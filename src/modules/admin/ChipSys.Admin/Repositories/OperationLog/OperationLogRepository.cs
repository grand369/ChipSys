using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.OperationLog;

namespace ChipSys.Admin.Repositories;

public class OperationLogRepository : LogRepositoryBase<OperationLogEntity>, IOperationLogRepository
{
    public OperationLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
