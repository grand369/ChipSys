using ChipSys.Admin.Core.Repositories;

namespace ChipSys.Admin.Domain.Role;

public interface IRoleRepository : IRepositoryBase<RoleEntity>
{
    /// <summary>
    /// ��ñ���ɫ���¼���ɫId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long id);

    /// <summary>
    /// ��õ�ǰ��ɫ���¼���ɫId
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long[] ids);
}
