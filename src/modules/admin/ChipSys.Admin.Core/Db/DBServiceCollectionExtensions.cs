using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Reflection;
using FreeSql;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Core.Startup;
using ChipSys.Admin.Core.Db.Transaction;

namespace ChipSys.Admin.Core.Db;

/// <summary>
/// ���ݿ���񼯺���չ
/// </summary>
public static class DBServiceCollectionExtensions
{
    /// <summary>
    /// ������ݿ�
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    /// <param name="hostAppOptions"></param>
    /// <returns></returns>
    public static void AddDb(this IServiceCollection services, IHostEnvironment env, HostAppOptions hostAppOptions)
    {
        var dbConfig = AppInfo.GetOptions<DbConfig>();
        var appConfig = AppInfo.GetOptions<AppConfig>();
        var user = services.BuildServiceProvider().GetService<IUser>();
        var freeSqlCloud = appConfig.DistributeKey.IsNull() ? new FreeSqlCloud() : new FreeSqlCloud(appConfig.DistributeKey);
        DbHelper.RegisterDb(freeSqlCloud, user, dbConfig, appConfig, hostAppOptions);

        //��������
        var masterDb = freeSqlCloud.Use(dbConfig.Key);
        services.AddSingleton(provider => masterDb);
        masterDb.Select<object>();

        //ע������ݿ�
        if (dbConfig.Dbs?.Length > 0)
        {
            foreach (var db in dbConfig.Dbs)
            {
                DbHelper.RegisterDb(freeSqlCloud, user, db, appConfig, hostAppOptions);
                //���е�ǰ��
                var currentDb = freeSqlCloud.Use(db.Key);
                currentDb.Select<object>();
            }
        }

        services.AddSingleton<IFreeSql>(freeSqlCloud);
        services.AddSingleton(freeSqlCloud);
        services.AddScoped<UnitOfWorkManagerCloud>();
    }

    /// <summary>
    /// ���TiDb���ݿ�
    /// </summary>
    /// <param name="_"></param>
    /// <param name="context"></param>
    /// <param name="version">�汾</param>
    public static void AddTiDb(this IServiceCollection _, HostAppContext context, string version = "8.0")
    {
        var dbConfig = AppInfo.GetOptions<DbConfig>();
        var _dicMySqlVersion = typeof(FreeSqlGlobalExtensions).GetField("_dicMySqlVersion", BindingFlags.NonPublic | BindingFlags.Static);
        var dicMySqlVersion = new ConcurrentDictionary<string, string>();
        dicMySqlVersion[dbConfig.ConnectionString] = version;
        _dicMySqlVersion.SetValue(new ConcurrentDictionary<string, string>(), dicMySqlVersion);
    }
}
