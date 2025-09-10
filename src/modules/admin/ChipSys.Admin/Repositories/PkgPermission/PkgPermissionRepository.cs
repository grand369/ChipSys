using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.PkgPermission;

namespace ChipSys.Admin.Repositories;

public class PkgPermissionRepository : AdminRepositoryBase<PkgPermissionEntity>, IPkgPermissionRepository
{
    public PkgPermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}
