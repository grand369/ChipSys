using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Supplier;

namespace ChipSys.Admin.Repositories;

public class SupplierRepository : AdminRepositoryBase<SupplierEntity>, ISupplierRepository
{
    public SupplierRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
