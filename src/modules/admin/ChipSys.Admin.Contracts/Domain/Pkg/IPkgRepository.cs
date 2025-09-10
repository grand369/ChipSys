using ChipSys.Admin.Core.Repositories;

namespace ChipSys.Admin.Domain.Pkg;

public interface IPkgRepository : IRepositoryBase<PkgEntity>
{
    /// <summary>
    /// ��ñ��ײͺ��¼��ײ�Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long id);

    /// <summary>
    /// ��õ�ǰ�ײͺ��¼��ײ�Id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long[] ids);
}
