using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChipSys.Admin.Services.Cache;

/// <summary>
/// ����ӿ�
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// �����б�
    /// </summary>
    /// <returns></returns>
    List<dynamic> GetList();

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <returns></returns>
    Task ClearAsync(string cacheKey);
}
