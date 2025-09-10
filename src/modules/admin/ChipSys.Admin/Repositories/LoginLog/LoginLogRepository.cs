using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.LoginLog;

namespace ChipSys.Admin.Repositories;

public class LoginLogRepository : LogRepositoryBase<LoginLogEntity>, ILoginLogRepository
{
    public LoginLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
