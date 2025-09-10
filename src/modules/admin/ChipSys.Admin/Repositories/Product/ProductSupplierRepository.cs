using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;

namespace ChipSys.Admin.Repositories;

public class ProductSupplierRepository : AdminRepositoryBase<ProductSupplierEntity>, IProductSupplierRepository
{
    public ProductSupplierRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
