using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.Pkg;

namespace ChipSys.Admin.Repositories;

public class PkgRepository : AdminRepositoryBase<PkgEntity>, IPkgRepository
{
    public PkgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }

    /// <summary>
    /// ��ñ���ɫ���¼���ɫId
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// ��õ�ǰ��ɫ���¼���ɫId
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long[] ids)
    {
        return await Select
        .Where(a => ids.Contains(a.Id))
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }
}
