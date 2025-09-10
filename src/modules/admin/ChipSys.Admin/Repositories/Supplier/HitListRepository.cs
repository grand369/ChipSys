using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;

namespace ChipSys.Admin.Repositories;

public class HitListRepository : AdminRepositoryBase<HitListEntity>, IHitListRepository
{
    public HitListRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
