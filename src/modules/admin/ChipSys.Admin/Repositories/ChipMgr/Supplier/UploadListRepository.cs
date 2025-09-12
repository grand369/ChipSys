using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;

namespace ChipSys.Admin.Repositories.ChipMgr.Supplier;

public class UploadListRepository : AdminRepositoryBase<UploadListEntity>, IUploadListRepository
{
    public UploadListRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
