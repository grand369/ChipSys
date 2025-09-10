using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Contracts.Domain.ChipMgr.ProductCategory;

namespace ChipSys.Admin.Repositories;

public class CategoryRepository : AdminRepositoryBase<CategoryEntity>, ICategoryRepository
{
    public CategoryRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
