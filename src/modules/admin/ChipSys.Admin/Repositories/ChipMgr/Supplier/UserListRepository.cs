using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;

namespace ChipSys.Admin.Repositories.ChipMgr.Supplier;

public class UserListRepository : AdminRepositoryBase<UserListEntity>, IUserListRepository
{
    public UserListRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
