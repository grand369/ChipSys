using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.DictType;

namespace ChipSys.Admin.Repositories;

public class DictionaryTypeRepository : AdminRepositoryBase<DictTypeEntity>, IDictTypeRepository
{
    public DictionaryTypeRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
