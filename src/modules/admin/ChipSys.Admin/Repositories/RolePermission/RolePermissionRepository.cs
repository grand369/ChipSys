using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.RolePermission;

namespace ChipSys.Admin.Repositories;

public class RolePermissionRepository : AdminRepositoryBase<RolePermissionEntity>, IRolePermissionRepository
{
    public RolePermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}
