using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Db.Transaction;
using ChipSys.Admin.Core.Repositories;

namespace ChipSys.Admin.Repositories
{
    /// <summary>
    /// Ȩ�޿�����ִ�
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class AdminRepositoryBase<TEntity> : RepositoryBase<TEntity> where TEntity : class
    {
        public AdminRepositoryBase(UnitOfWorkManagerCloud uowm) : base(DbKeys.AdminDb, uowm) 
        {
            
        }
    }
}
