using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Domain.Api;

namespace ChipSys.Admin.Repositories;

public class ApiRepository : AdminRepositoryBase<ApiEntity>, IApiRepository
{
    public ApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}
