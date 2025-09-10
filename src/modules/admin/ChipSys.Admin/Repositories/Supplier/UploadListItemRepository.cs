using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;

namespace ChipSys.Admin.Repositories;

public class UploadListItemRepository : AdminRepositoryBase<UploadListItemEntity>, IUploadListItemRepository
{
    public UploadListItemRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
