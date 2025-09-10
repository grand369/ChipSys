using Autofac;
using FreeScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using Savorboard.CAP.InMemoryMessageQueue;
using ChipSys.Admin.Core;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Db;
using ChipSys.Admin.Core.Startup;
using ChipSys.Admin.Services.TaskScheduler;
using ChipSys.Admin.Tools.TaskScheduler;
using ChipSys.ApiUI;
using ChipSys.Common.Helpers;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Core.Extensions;
using DotNetCore.CAP.Messages;
using System.Text.Encodings.Web;

// ����Ӧ��ʵ��
new HostApp(new HostAppOptions
{
    //ǰ������FreeSql
    ConfigurePreFreeSql = (freeSql, dbConfig) =>
    {
        freeSql.UseJsonMap(); //����JsonMap����
        freeSql.UseLogDb(dbConfig); //ʹ����־���ݿ�
    },
    //����FreeSql������
    ConfigureFreeSqlBuilder = (freeSqlBuilder, dbConfig) =>
    {
        if (dbConfig.Type == FreeSql.DataType.QuestDb)
        {
            freeSqlBuilder.UseQuestDbRestAPI("http://localhost:9000", "admin", "quest");
        }
    },
    //����FreeSql
    ConfigureFreeSql = (freeSql, dbConfig) =>
    {
        if (dbConfig.Key == DbKeys.TaskDb)
        {
            freeSql.SyncSchedulerStructure(dbConfig, TaskSchedulerServiceExtensions.ConfigureScheduler);
        }
    },
    //����ǰ�÷���
    ConfigurePreServices = context =>
    {
        DbKeys.LogDb = "logdb";

        context.Services.Configure<TaskSchedulerConfig>(context.Configuration.GetSection("TaskScheduler"));
    },
    //���ú��÷���
    ConfigurePostServices = context =>
    {
        //���cap�¼�����
        var appConfig = AppInfo.GetRequiredService<AppConfig>(false);
        Assembly[] assemblies = AssemblyHelper.GetAssemblyList(appConfig.AssemblyNames);

        //var dbConfig = AppInfo.GetRequiredService<DbConfig>(false);
        //var rabbitMQ = context.Configuration.GetSection("CAP:RabbitMq").Get<RabbitMQOptions>();
        context.Services.AddCap(config =>
        {
            config.DefaultGroupName = "ChipSys.Admin";
            //�����׶β�ͬ������Ա����Ϣ���֣�����ͨ�����ð汾��ʵ��
            config.Version = "v1";
            config.FailedRetryCount = 5;
            config.FailedRetryInterval = 15;
            config.EnablePublishParallelSend = true;
            config.UseStorageLock = true;
            config.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            config.UseInMemoryStorage();
            config.UseInMemoryMessageQueue();
            //config.UseMySql(dbConfig.ConnectionString);
            //config.UseRabbitMQ(mqConfig => {
            //    mqConfig.HostName = rabbitMQ.HostName;
            //    mqConfig.Port = rabbitMQ.Port;
            //    mqConfig.UserName = rabbitMQ.UserName;
            //    mqConfig.Password = rabbitMQ.Password;
            //    mqConfig.ExchangeName = rabbitMQ.ExchangeName;
            //});

            config.FailedThresholdCallback = failed =>
            {
                AppInfo.Log.Error($@"��Ϣ����ʧ�ܣ�����: {failed.MessageType}, 
������ {config.FailedRetryCount} ����ʧ�ܣ����˹�������Ϣ����: {failed.Message.GetName()}");
            };

            config.UseDashboard();
        }).AddSubscriberAssembly(assemblies);

        //����������
        context.Services.AddTaskScheduler(DbKeys.TaskDb, options =>
        {
            options.ConfigureFreeSql = TaskSchedulerServiceExtensions.ConfigureScheduler;

            //�����������
            options.ConfigureFreeSchedulerBuilder = freeSchedulerBuilder =>
            {
                static void OnExecuting(TaskInfo task)
                {
                    if (task.Topic?.StartsWith("[shell]") == true)
                    {
                        TaskSchedulerServiceExtensions.ExecuteGrpc(task);
                    }
                }

                freeSchedulerBuilder
                    .OnExecuting(task => OnExecuting(task))
                    .OnExecuted((task, taskLog) =>
                    {
                        try
                        {
                            if (!taskLog.Success)
                            {
                                var taskService = AppInfo.GetRequiredService<TaskService>();
                                var taskInfo = taskService.GetAsync(task.Id).Result;

                                //ʧ������
                                TaskSchedulerServiceExtensions.FailedRetry(taskInfo, task, taskLog, OnExecuting);

                                //���͸澯�ʼ�
                                TaskSchedulerServiceExtensions.SendAlarmEmail(taskInfo, task, taskLog);
                            }
                        }
                        catch (Exception ex)
                        {
                            AppInfo.Log.Error(ex);
                        }
                    });
            };
        });

        //��ӻ�����֤
        context.Services.AddSlideCaptcha();
    },
    //����Autofac����
    ConfigureAutofacContainer = (builder, context) => 
    {
        builder.RegisterGeneric(typeof(AdminRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
        builder.RegisterGeneric(typeof(LogRepositoryBase<>)).InstancePerLifetimeScope().PropertiesAutowired();
    },
    //����Mvc
    ConfigureMvcBuilder = (builder, context) => { },
    //���ú����м��
    ConfigurePostMiddleware = context =>
    {
        var app = context.App;
        var env = app.Environment;
        var appConfig = app.Services.GetService<AppConfig>();

        #region �°�Api�ĵ�

        if (env.IsDevelopment() || appConfig.ApiUI.Enable)
        {
            app.UseApiUI(options =>
            {
                options.RoutePrefix = appConfig.ApiUI.RoutePrefix;
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{options.RoutePrefix}/swagger/{project.Code.ToLower()}/swagger.json",
                        project.Name);
                });
            });
        }

        #endregion
    },
    ConfigureSwaggerUI = options =>
    {
        //options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.Full);
    }
}).Run(args, typeof(Program).Assembly);

#if DEBUG
public partial class Program
{
}
#endif
