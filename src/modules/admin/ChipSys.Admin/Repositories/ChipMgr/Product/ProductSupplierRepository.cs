using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;

namespace ChipSys.Admin.Repositories.ChipMgr.Product;

public class ProductSupplierRepository : AdminRepositoryBase<ProductSupplierEntity>, IProductSupplierRepository
{
    public ProductSupplierRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
