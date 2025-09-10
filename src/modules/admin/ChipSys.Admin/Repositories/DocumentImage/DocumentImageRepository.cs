using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.DocImage;

namespace ChipSys.Admin.Repositories;

public class DocumentImageRepository : AdminRepositoryBase<DocImageEntity>, IDocImageRepository
{
    public DocumentImageRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
