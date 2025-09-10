using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.UserRole;

namespace ChipSys.Admin.Repositories;

public class UserRoleRepository : AdminRepositoryBase<UserRoleEntity>, IUserRoleRepository
{
    public UserRoleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}
