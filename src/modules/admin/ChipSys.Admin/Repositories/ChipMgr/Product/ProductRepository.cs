using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.Product;

namespace ChipSys.Admin.Repositories.ChipMgr.Product;

public class ProductRepository : AdminRepositoryBase<ProductEntity>, IProductRepository
{
    public ProductRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
