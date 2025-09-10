using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Core.Repositories;

namespace ChipSys.Admin.Repositories;

/// <summary>
/// ÈÕÖ¾¿â»ù´¡²Ö´¢
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class LogRepositoryBase<TEntity> : RepositoryBase<TEntity> where TEntity : class
{
    public LogRepositoryBase(UnitOfWorkManagerCloud uowm) : base(DbKeys.LogDb, uowm) 
    {
        
    }
}
