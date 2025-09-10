using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain;

namespace ChipSys.Admin.Repositories;

public class FileRepository : AdminRepositoryBase<FileEntity>, IFileRepository
{
    public FileRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
