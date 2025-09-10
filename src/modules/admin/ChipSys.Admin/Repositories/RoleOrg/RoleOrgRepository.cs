using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain;
using ChipSys.Admin.Domain.RoleOrg;

namespace ChipSys.Admin.Repositories;

public class RoleOrgRepository : AdminRepositoryBase<RoleOrgEntity>, IRoleOrgRepository
{
    public RoleOrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}
