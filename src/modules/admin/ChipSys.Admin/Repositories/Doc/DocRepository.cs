using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.Doc;

namespace ChipSys.Admin.Repositories;

public class DocRepository : AdminRepositoryBase<DocEntity>, IDocRepository
{
    public DocRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
