using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Reflection;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Configs;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Common.Helpers;
using Microsoft.Extensions.Options;

namespace ChipSys.Admin.Services.Cache;

/// <summary>
/// �������
/// </summary>
[Order(80)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class CacheService : BaseService, ICacheService, IDynamicApi
{
    private readonly AppConfig _appConfig;
    public CacheService(IOptions<AppConfig> appConfig)
    {
        _appConfig = appConfig.Value;
    }

    /// <summary>
    /// ��ѯ�б�
    /// </summary>
    /// <returns></returns>
    public List<dynamic> GetList()
    {
        var list = new List<dynamic>();

        Assembly[] assemblies = AssemblyHelper.GetAssemblyList(_appConfig.AssemblyNames);

        foreach (Assembly assembly in assemblies)
        {
            var types = assembly.GetExportedTypes().Where(a => a.GetCustomAttribute<ScanCacheKeysAttribute>(false) != null);
            foreach (Type type in types)
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                foreach (FieldInfo field in fields)
                {
                    var descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                    list.Add(new
                    {
                        field.Name,
                        Value = field.GetRawConstantValue().ToString(),
                        descriptionAttribute?.Description
                    });
                }
            }
        }

        return list;
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="cacheKey">�����</param>
    /// <returns></returns>
    public async Task ClearAsync(string cacheKey)
    {
        Logger.LogWarning($"{User.Id}.{User.UserName}�������[{cacheKey}]");
        await Cache.DelByPatternAsync(cacheKey + "*");
    }
}
