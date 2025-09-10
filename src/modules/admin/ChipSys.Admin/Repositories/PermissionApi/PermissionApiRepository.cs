using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.PermissionApi;

namespace ChipSys.Admin.Repositories;

public class PermissionApiRepository : AdminRepositoryBase<PermissionApiEntity>, IPermissionApiRepository
{
    public PermissionApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}
