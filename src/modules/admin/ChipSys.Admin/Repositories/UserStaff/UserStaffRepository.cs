using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.UserStaff;

namespace ChipSys.Admin.Repositories;

public class UserStaffRepository : AdminRepositoryBase<UserStaffEntity>, IUserStaffRepository
{
    public UserStaffRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}
