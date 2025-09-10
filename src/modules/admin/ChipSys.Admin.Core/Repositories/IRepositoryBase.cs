using FreeSql;
using System.Linq.Expressions;
using ChipSys.Admin.Core.Auth;

namespace ChipSys.Admin.Core.Repositories;

public interface IRepositoryBase<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
{
    IUser User { get; set; }

    /// <summary>
    /// ���Dto
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="id">����</param>
    /// <returns></returns>
    Task<TDto> GetAsync<TDto>(TKey id);

    /// <summary>
    /// ����������ȡDto
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp);

    /// <summary>
    /// ����������ȡʵ��
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp);

    /// <summary>
    /// ��ɾ��
    /// </summary>
    /// <param name="id">����</param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(TKey id);

    /// <summary>
    /// ������ɾ��
    /// </summary>
    /// <param name="ids">��������</param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(TKey[] ids);

    /// <summary>
    /// ��ɾ��
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="disableGlobalFilterNames">����ȫ�ֹ�������</param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames);

    /// <summary>
    /// �ݹ�ɾ��
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="disableGlobalFilterNames">����ȫ�ֹ�������</param>
    /// <returns></returns>
    Task<bool> DeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames);

    /// <summary>
    /// �ݹ���ɾ��
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="disableGlobalFilterNames">����ȫ�ֹ�������</param>
    /// <returns></returns>
    Task<bool> SoftDeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames);
}

public interface IRepositoryBase<TEntity> : IRepositoryBase<TEntity, long> where TEntity : class
{
}
