using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.UserOrg;

namespace ChipSys.Admin.Repositories;

public class UserOrgRepository : AdminRepositoryBase<UserOrgEntity>, IUserOrgRepository
{
    public UserOrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
        
    }

    /// <summary>
    /// ���������Ƿ���Ա��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(long id)
    {
        return await Select.Where(a => a.OrgId == id).AnyAsync();
    }

    /// <summary>
    /// �����б����Ƿ���Ա��
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(List<long> idList)
    {
        return await Select.Where(a => idList.Contains(a.OrgId)).AnyAsync();
    }
}
