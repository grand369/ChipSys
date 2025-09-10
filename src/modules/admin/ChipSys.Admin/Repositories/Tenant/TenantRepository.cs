using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.Tenant;

namespace ChipSys.Admin.Repositories;

public class TenantRepository : AdminRepositoryBase<TenantEntity>, ITenantRepository
{
    public TenantRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}
