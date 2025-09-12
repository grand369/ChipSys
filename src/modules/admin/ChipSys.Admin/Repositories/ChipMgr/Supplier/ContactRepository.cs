using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;

namespace ChipSys.Admin.Repositories.ChipMgr.Supplier;

public class ContactRepository : AdminRepositoryBase<ContactEntity>, IContactRepository
{
    public ContactRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
