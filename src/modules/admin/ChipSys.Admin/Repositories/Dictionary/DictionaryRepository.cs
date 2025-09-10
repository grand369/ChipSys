using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.Dict;

namespace ChipSys.Admin.Repositories;

public class DictionaryRepository : AdminRepositoryBase<DictEntity>, IDictRepository
{
    public DictionaryRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
