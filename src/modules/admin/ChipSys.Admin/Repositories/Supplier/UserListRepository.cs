using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;

namespace ChipSys.Admin.Repositories;

public class UserListRepository : AdminRepositoryBase<UserListEntity>, IUserListRepository
{
    public UserListRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
