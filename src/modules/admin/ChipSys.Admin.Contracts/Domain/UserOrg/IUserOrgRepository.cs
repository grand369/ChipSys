using ChipSys.Admin.Core.Repositories;

namespace ChipSys.Admin.Domain.UserOrg;

public interface IUserOrgRepository : IRepositoryBase<UserOrgEntity>
{
    /// <summary>
    /// ���������Ƿ���Ա��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> HasUser(long id);

    /// <summary>
    /// �����б����Ƿ���Ա��
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    Task<bool> HasUser(List<long> idList);
}
