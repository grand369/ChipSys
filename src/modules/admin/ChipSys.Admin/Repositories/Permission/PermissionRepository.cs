using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.Permission;

namespace ChipSys.Admin.Repositories;

public class PermissionRepository : AdminRepositoryBase<PermissionEntity>, IPermissionRepository
{
    public PermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
