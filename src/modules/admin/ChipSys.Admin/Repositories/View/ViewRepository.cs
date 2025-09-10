using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.View;

namespace ChipSys.Admin.Repositories;

public class ViewRepository : AdminRepositoryBase<ViewEntity>, IViewRepository
{
    public ViewRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}
