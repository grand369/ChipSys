using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.User;

namespace ChipSys.Admin.Repositories;

public class UserRepository : AdminRepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {

    }
}
