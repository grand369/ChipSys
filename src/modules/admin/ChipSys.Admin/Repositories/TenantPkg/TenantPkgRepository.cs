using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.TenantPkg;

namespace ChipSys.Admin.Repositories;

public class TenantPkgRepository : AdminRepositoryBase<TenantPkgEntity>, ITenantPkgRepository
{
    public TenantPkgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}
