using StackExchange.Profiling;
using System.Reflection;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using Yitter.IdGenerator;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Entities;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Auth;
using ChipSys.Common.Helpers;
using ChipSys.Admin.Core.Db.Data;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Startup;
using ChipSys.Admin.Domain.Role;
using ChipSys.Admin.Domain.User;

namespace ChipSys.Admin.Core.Db;

/// <summary>
/// ���ݿ������
/// </summary>
public class DbHelper
{
    /// <summary>
    /// ƫ��ʱ��
    /// </summary>
    private static TimeSpan timeOffset;

    public static TimeSpan TimeOffset { get => timeOffset; set => timeOffset = value; }

    /// <summary>
    /// ���ݿ�ʱ��
    /// </summary>
    public static DateTime ServerTime => DateTime.Now.Subtract(timeOffset);

    /// <summary>
    /// �������ݿ�
    /// </summary>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    public async static Task CreateDatabaseAsync(DbConfig dbConfig)
    {
        if (!dbConfig.CreateDb || dbConfig.Type == DataType.Sqlite)
        {
            return;
        }

        var db = new FreeSqlBuilder()
                .UseConnectionString(dbConfig.Type, dbConfig.CreateDbConnectionString)
                .Build();

        try
        {
            Console.WriteLine($"{Environment.NewLine}create database {dbConfig.Key} started");
            var filePath = Path.Combine(AppContext.BaseDirectory, dbConfig.CreateDbSqlFile).ToPath();
            if (File.Exists(filePath))
            {
                var createDbSql = FileHelper.ReadFile(filePath);
                if (createDbSql.NotNull())
                {
                    dbConfig.CreateDbSql = createDbSql;
                }
            }

            await db.Ado.ExecuteNonQueryAsync(dbConfig.CreateDbSql);
            Console.WriteLine($"create {dbConfig.Key} database succeed");
        }
        catch (Exception e)
        {
            Console.WriteLine($"create {dbConfig.Key} database failed.\n {e.Message}");
        }
    }

    /// <summary>
    /// ���ָ�����򼯱�ʵ��
    /// </summary>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    public static List<Type> GetEntityTypes(DbConfig dbConfig)
    {
        var entityTypes = new List<Type>();

        if (!(dbConfig.AssemblyNames?.Length > 0))
        {
            return entityTypes;
        }

        foreach (var assemblyName in dbConfig.AssemblyNames)
        {
            var assembly = AssemblyHelper.GetAssembly(assemblyName);
            if (assembly == null) continue;

            //entityTypes.AddRange(
            //    assembly.GetExportedTypes().Where(type => 
            //        type.GetCustomAttribute<TableAttribute>() is { DisableSyncStructure: false })
            //);

            var types = assembly.GetExportedTypes()
           .Where(type => type.GetCustomAttribute<TableAttribute>() is { DisableSyncStructure: false });

            // Ӧ�����ݿ�ɸѡ�߼����ų����ȣ�
            if (dbConfig.ExcludeEntityDbs?.Length > 0 || dbConfig.IncludeEntityDbs?.Length > 0)
            {
                types = types.Where(type =>
                {
                    var dbAttr = type.GetCustomAttribute<DatabaseAttribute>();
                    var dbName = dbAttr?.Name;

                    // 1. ��ִ���ų����
                    if (dbConfig.ExcludeEntityDbs?.Length > 0 && dbName != null)
                    {
                        if (dbConfig.ExcludeEntityDbs.Contains(dbName))
                        {
                            return false; // ���ų���ʵ��ֱ�ӹ��˵�
                        }
                    }

                    // 2. ��ִ�а������
                    if (dbConfig.IncludeEntityDbs?.Length > 0)
                    {
                        return dbName != null && dbConfig.IncludeEntityDbs.Contains(dbName);
                    }

                    return true; // ��û���ų�Ҳ���������ʵ�屣��
                });
            }

            entityTypes.AddRange(types);
        }

        return entityTypes;
    }

    /// <summary>
    /// ����ʵ��
    /// </summary>
    /// <param name="db"></param>
    /// <param name="appConfig"></param>
    /// <param name="dbConfig"></param>
    public static void ConfigEntity(IFreeSql db, AppConfig appConfig = null, DbConfig dbConfig = null)
    {
        //�⻧���ɺͲ����⻧Id
        if (!appConfig.Tenant)
        {
            var iTenant = nameof(ITenant);
            var tenantId = nameof(ITenant.TenantId);

            //���ָ�����򼯱�ʵ��
            var entityTypes = GetEntityTypes(dbConfig);

            var tenantEntities = entityTypes?
            .Where(type => type.GetInterfaces().Any(a => a.Name == iTenant))
            .ToList();

            if(tenantEntities?.Count > 0)
            {
                foreach (var entityType in tenantEntities)
                {
                    db.CodeFirst.Entity(entityType, a => a.Ignore(tenantId));
                }
            }
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="e"></param>
    /// <param name="timeOffset"></param>
    /// <param name="user"></param>
    /// <param name="dbConfig"></param>
    public static void AuditValue(AuditValueEventArgs e, TimeSpan timeOffset, IUser user, DbConfig dbConfig)
    {
        if (e.Property == null)
        {
            return;
        }

        //���ݿ�ʱ��
        if ((e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
        && e.Property.GetCustomAttribute<ServerTimeAttribute>(false) is ServerTimeAttribute serverTimeAttribute)
        {
            if(!dbConfig.ForceUpdate && !serverTimeAttribute.CanInsert && e.AuditValueType is AuditValueType.Insert)
            {
                return;
            }

            if((e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default) || serverTimeAttribute.CanUpdate)
            {
                e.Value = DateTime.Now.Subtract(timeOffset);
            }
        }

        //ѩ��Id
        if (e.Column.CsType == typeof(long)
        && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is SnowflakeAttribute snowflakeAttribute
        && snowflakeAttribute.Enable && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
        {
            e.Value = YitIdHelper.NextId();
        }

        //����Guid
        if (e.Column.CsType == typeof(Guid)
        && e.Property.GetCustomAttribute<OrderGuidAttribute>(false) is OrderGuidAttribute orderGuidAttribute
        && orderGuidAttribute.Enable && (e.Value == null || (Guid)e.Value == default || (Guid?)e.Value == default))
        {
            e.Value = FreeUtil.NewMongodbId();
        }

        if (user == null || user.Id <= 0)
        {
            return;
        }

        if (e.AuditValueType is AuditValueType.Insert or AuditValueType.InsertOrUpdate)
        {
            switch (e.Property.Name)
            {
                case "CreatedUserId":
                case "OwnerId":
                case "MemberId":
                    if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                    {
                        e.Value = user.Id;
                    }
                    break;
                case "CreatedUserName":
                    if (e.Value == null || ((string)e.Value).IsNull())
                    {
                        e.Value = user.UserName;
                    }
                    break;
                case "CreatedUserRealName":
                    if (e.Value == null || ((string)e.Value).IsNull())
                    {
                        e.Value = user.Name;
                    }
                    break;
                case "OwnerOrgId":
                    if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                    {
                        e.Value = user.DataPermission?.OrgId;
                    }
                    break;
                case "OwnerOrgName":
                    if (e.Value == null || ((string)e.Value).IsNull())
                    {
                        e.Value = user.DataPermission?.OrgName;
                    }
                    break;
                case "TenantId":
                    if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                    {
                        e.Value = user.TenantId;
                    }
                    break;
            }
        }

        if ((e.AuditValueType is AuditValueType.Update or AuditValueType.InsertOrUpdate) || dbConfig.ForceUpdate)
        {
            switch (e.Property.Name)
            {
                case "ModifiedUserId":
                    e.Value = user.Id;
                    break;
                case "ModifiedUserName":
                    e.Value = user.UserName;
                    break;
                case "ModifiedUserRealName":
                    e.Value = user.Name;
                    break;
            }
        }
    }

    private static void SyncStructureAfter(object? s, SyncStructureAfterEventArgs e)
    {
        if (e.Sql.NotNull())
        {
            Console.WriteLine("sync structure sql:\n" + e.Sql);
        }
    }

    /// <summary>
    /// ͬ���ṹ
    /// </summary>
    /// <param name="db"></param>
    /// <param name="msg"></param>
    /// <param name="dbConfig"></param>
    /// <param name="configureFreeSqlSyncStructure"></param>
    public static void SyncStructure(IFreeSql db, string msg = null, 
        DbConfig dbConfig = null, 
        Action<IFreeSql, DbConfig> configureFreeSqlSyncStructure = null)
    {
        //��ӡ�ṹ�ȶԽű�
        //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
        //Console.WriteLine($"{Environment.NewLine}" + dDL);

        //��ӡ�ṹͬ���ű�
        if (dbConfig.SyncStructureSql)
        {
            db.Aop.SyncStructureAfter += SyncStructureAfter;
        }

        // ͬ���ṹ
        var dbType = dbConfig.Type.ToString();
        Console.WriteLine($"{Environment.NewLine}{(msg.NotNull() ? msg : $"sync {dbConfig.Key} {dbType} structure")} started");

        //���ָ�����򼯱�ʵ��
        var entityTypes = GetEntityTypes(dbConfig);

        var batchSize = dbConfig.SyncStructureEntityBatchSize;
        batchSize = batchSize <= 1 ? 1 : batchSize;

        if(entityTypes?.Count > 0)
        {
            if (batchSize == 1)
            {
                foreach (var entityType in entityTypes)
                {
                    db.CodeFirst.SyncStructure(entityType);
                }
            }
            else
            {
                for (int i = 0, count = entityTypes.Count; i < count; i += batchSize)
                {
                    var batchEntityTypes = entityTypes.GetRange(i, Math.Min(batchSize, count - i));
                    db.CodeFirst.SyncStructure(batchEntityTypes.ToArray());
                }
            }
        }

        //�Զ���Ǩ�ƽṹ
        configureFreeSqlSyncStructure?.Invoke(db, dbConfig);

        if (dbConfig.SyncStructureSql)
        {
            db.Aop.SyncStructureAfter -= SyncStructureAfter;
        }

        Console.WriteLine($"{(msg.NotNull() ? msg : $"sync {dbConfig.Key} {dbType} structure")} succeed");
    }

    private static void SyncDataCurdBefore(object? s, CurdBeforeEventArgs e)
    {
        if (e.Sql.NotNull())
        {
            Console.WriteLine($"{e.Sql}{Environment.NewLine}");
        }
    }

    /// <summary>
    /// ͬ������
    /// </summary>
    /// <param name="db"></param>
    /// <param name="dbConfig"></param>
    /// <param name="appConfig"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task SyncDataAsync(
        IFreeSql db,
        DbConfig dbConfig = null,
        AppConfig appConfig = null
    )
    {
        try
        {
            Console.WriteLine($"{Environment.NewLine}sync {dbConfig.Key} data started");

            if (dbConfig.AssemblyNames?.Length > 0)
            {
                var user = dbConfig.SyncDataUser;

                // ͬ��������Ʒ���
                void SyncDataAuditValue(object s, AuditValueEventArgs e)
                {
                    if (e.Property == null)
                    {
                        return;
                    }

                    if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) is ServerTimeAttribute serverTimeAttribute
                           && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                           && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
                    {
                        if (!dbConfig.ForceUpdate && !serverTimeAttribute.CanInsert && e.AuditValueType is AuditValueType.Insert)
                        {
                            return;
                        }

                        if ((e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default) || serverTimeAttribute.CanUpdate)
                        {
                            e.Value = DateTime.Now.Subtract(timeOffset);
                        }
                    }

                    if (e.Column.CsType == typeof(long)
                    && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null
                    && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
                    {
                        e.Value = YitIdHelper.NextId();
                    }

                    if (user == null || user.Id <= 0)
                    {
                        return;
                    }

                    if (e.AuditValueType is AuditValueType.Insert or AuditValueType.InsertOrUpdate)
                    {
                        switch (e.Property.Name)
                        {
                            case "CreatedUserId":
                                if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                                {
                                    e.Value = user.Id;
                                }
                                break;
                            case "CreatedUserName":
                                if (e.Value == null || ((string)e.Value).IsNull())
                                {
                                    e.Value = user.UserName;
                                }
                                break;
                            case "CreatedUserRealName":
                                if (e.Value == null || ((string)e.Value).IsNull())
                                {
                                    e.Value = user.Name;
                                }
                                break;
                            case "TenantId":
                                if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                                {
                                    e.Value = user.TenantId;
                                }
                                break;
                        }
                    }

                    if ((e.AuditValueType is AuditValueType.Update or AuditValueType.InsertOrUpdate) || dbConfig.ForceUpdate)
                    {
                        switch (e.Property.Name)
                        {
                            case "ModifiedUserId":
                                if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                                {
                                    e.Value = user.Id;
                                }
                                break;
                            case "ModifiedUserName":
                                if (e.Value == null || ((string)e.Value).IsNull())
                                {
                                    e.Value = user.UserName;
                                }
                                break;
                            case "ModifiedUserRealName":
                                if (e.Value == null || ((string)e.Value).IsNull())
                                {
                                    e.Value = user.Name;
                                }
                                break;
                        }
                    }
                }

                db.Aop.AuditValue += SyncDataAuditValue;

                if (dbConfig.SyncDataCurd)
                {
                    db.Aop.CurdBefore += SyncDataCurdBefore;
                }

                Assembly[] assemblies = AssemblyHelper.GetAssemblyList(dbConfig.AssemblyNames);

                List<ISyncData> syncDatas = assemblies.Select(assembly => assembly.GetTypes()
                .Where(x => typeof(ISyncData).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(registerTypes => registerTypes.Select(registerType => (ISyncData)Activator.CreateInstance(registerType))).ToList();

                foreach (ISyncData syncData in syncDatas)
                {
                    await syncData.SyncDataAsync(db, dbConfig, appConfig);
                }

                if (dbConfig.SyncDataCurd)
                {
                    db.Aop.CurdBefore -= SyncDataCurdBefore;
                }

                db.Aop.AuditValue -= SyncDataAuditValue;
            }

            Console.WriteLine($"sync {dbConfig.Key} data succeed{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            throw new Exception($"sync {dbConfig.Key} data failed.\n{ex.Message}");
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="db"></param>
    /// <param name="appConfig"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig = null, DbConfig dbConfig = null)
    {
        try
        {
            Console.WriteLine($"{Environment.NewLine}generate {dbConfig.Key} data started");

            if (dbConfig.AssemblyNames?.Length > 0)
            {
                Assembly[] assemblies = AssemblyHelper.GetAssemblyList(dbConfig.AssemblyNames);

                List<IGenerateData> generateDatas = assemblies.Select(assembly => assembly.GetTypes()
                .Where(x => typeof(IGenerateData).GetTypeInfo().IsAssignableFrom(x.GetTypeInfo()) && x.GetTypeInfo().IsClass && !x.GetTypeInfo().IsAbstract))
                .SelectMany(registerTypes => registerTypes.Select(registerType => (IGenerateData)Activator.CreateInstance(registerType))).ToList();

                foreach (IGenerateData generateData in generateDatas)
                {
                    await generateData.GenerateDataAsync(db, appConfig);
                }
            }

            Console.WriteLine($"generate {dbConfig.Key} data succeed{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            throw new Exception($"generate {dbConfig.Key} data failed��\n{ex.Message}{Environment.NewLine}");
        }
    }

    /// <summary>
    /// ע�����ݿ�
    /// </summary>
    /// <param name="freeSqlCloud"></param>
    /// <param name="user"></param>
    /// <param name="dbConfig"></param>
    /// <param name="appConfig"></param>
    /// <param name="hostAppOptions"></param>
    public static void RegisterDb(
        FreeSqlCloud freeSqlCloud,
        IUser user,
        DbConfig dbConfig,
        AppConfig appConfig,
        HostAppOptions hostAppOptions
    )
    {
        //ע�����ݿ�
        var idelTime = dbConfig.IdleTime.HasValue && dbConfig.IdleTime.Value > 0 ? TimeSpan.FromMinutes(dbConfig.IdleTime.Value) : TimeSpan.MaxValue;
        freeSqlCloud.Register(dbConfig.Key, () =>
        {
            //�������ݿ�
            if (dbConfig.CreateDb)
            {
                CreateDatabaseAsync(dbConfig).Wait();
            }

            var providerType = dbConfig.ProviderType.NotNull() ? Type.GetType(dbConfig.ProviderType) : null;
            var freeSqlBuilder = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString, providerType)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            if (dbConfig.SlaveList?.Length > 0)
            {
                var slaveList = dbConfig.SlaveList.Select(a => a.ConnectionString).ToArray();
                var slaveWeightList = dbConfig.SlaveList.Select(a => a.Weight).ToArray();
                freeSqlBuilder.UseSlave(slaveList).UseSlaveWeight(slaveWeightList);
            }

            #region ������������

            if (dbConfig.MonitorCommand)
            {
                freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                {
                    //Console.WriteLine($"{cmd.CommandText}\n{traceLog}{Environment.NewLine}");
                    Console.WriteLine($"{cmd.CommandText}{Environment.NewLine}");
                });
            }

            #endregion ������������

            hostAppOptions?.ConfigureFreeSqlBuilder?.Invoke(freeSqlBuilder, dbConfig);

            var fsql = freeSqlBuilder.Build();

            hostAppOptions?.ConfigurePreFreeSql?.Invoke(fsql, dbConfig);

            //��������
            if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
            {
                GenerateDataAsync(fsql, appConfig, dbConfig).Wait();
            }

            //���������ʱ��
            var serverTime = fsql.Ado.QuerySingle(() => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            TimeOffset = timeOffset;

            if (dbConfig.Type == DataType.Oracle)
            {
                fsql.CodeFirst.IsSyncStructureToUpper = true;
            }

            //����ʵ��
            ConfigEntity(fsql, appConfig, dbConfig);

            //ͬ���ṹ
            if (dbConfig.SyncStructure)
            {
                SyncStructure(fsql, dbConfig: dbConfig, configureFreeSqlSyncStructure: hostAppOptions?.ConfigureFreeSqlSyncStructure);
            }

            //ͬ������
            if (dbConfig.SyncData)
            {
                SyncDataAsync(fsql, dbConfig, appConfig).Wait();
            }

            //�������
            fsql.Aop.AuditValue += (s, e) =>
            {
                AuditValue(e, timeOffset, user, dbConfig);
            };

            #region ������

            //��ɾ��������
            fsql.GlobalFilter.ApplyOnly<IDelete>(FilterNames.Delete, a => a.IsDeleted == false);

            //�⻧������
            if (appConfig.Tenant)
            {
                fsql.GlobalFilter.ApplyOnly<ITenant>(FilterNames.Tenant, a => a.TenantId == user.TenantId);
            }

            //��Ա������
            fsql.GlobalFilter.ApplyOnlyIf<IMember>(FilterNames.Member,
                () =>
                {
                    if (user?.Id > 0 && user.Type != UserType.Member)
                    {
                        return false;
                    }
                    return true;
                },
                a => a.MemberId == user.Id
            );

            //����Ȩ�޹�����
            fsql.GlobalFilter.ApplyOnlyIf<IData>(FilterNames.Self,
                () =>
                {
                    var dataPermission = user.DataPermission;
                    if (dataPermission != null && (dataPermission.DataScope == DataScope.All || dataPermission.OrgIds.Count > 0))
                    {
                        return false;
                    }
                    return true;
                },
                a => a.OwnerId == user.Id
            );
            fsql.GlobalFilter.ApplyOnlyIf<IData>(FilterNames.Data,
                () =>
                {
                    var dataPermission = user.DataPermission;
                    if (dataPermission == null || (dataPermission != null && (dataPermission.DataScope == DataScope.All || dataPermission.OrgIds.Count == 0)))
                    {
                        return false;
                    }
                    return true;
                },
                a => a.OwnerId == user.Id || user.DataPermission.OrgIds.Contains(a.OwnerOrgId.Value)
            );

            #endregion

            #region ����Curd����

            if (dbConfig.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    if (appConfig.MiniProfiler)
                    {
                        MiniProfiler.Current.CustomTiming("CurdBefore", e.Sql);
                    }
                    Console.WriteLine($"{e.Sql}{Environment.NewLine}");
                };
                fsql.Aop.CurdAfter += (s, e) =>
                {
                    if (appConfig.MiniProfiler)
                    {
                        MiniProfiler.Current.CustomTiming("CurdAfter", $"{e.ElapsedMilliseconds}");
                    }
                };
            }

            #endregion ����Curd����

            hostAppOptions?.ConfigureFreeSql?.Invoke(fsql, dbConfig);

            return fsql;
        }, idelTime);
    }
}
