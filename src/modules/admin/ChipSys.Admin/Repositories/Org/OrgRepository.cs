using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.Org;

namespace ChipSys.Admin.Repositories;

public class OrgRepository : AdminRepositoryBase<OrgEntity>, IOrgRepository
{
    public OrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }

    /// <summary>
    /// ��ñ����ź��¼�����Id
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
}
