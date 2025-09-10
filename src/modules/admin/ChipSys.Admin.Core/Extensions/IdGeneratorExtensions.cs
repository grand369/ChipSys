using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using Yitter.IdGenerator;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Tools.Cache;

namespace ChipSys.Admin.Core.Extensions;

/// <summary>
/// Id��������չ
/// </summary>
public static class IdGeneratorExtensions
{
    private static bool _isSet = false;
    private static readonly object _locker = new();

    /// <summary>
    /// ���Id������
    /// </summary>
    /// <param name="services"></param>
    public static void AddIdGenerator(this IServiceCollection services)
    {
        var idGeneratorConfig = AppInfo.GetOptions<AppConfig>().IdGenerator;

        if (_isSet)
            return;
            //throw new InvalidOperationException("ֻ�������һ��Id������");

        lock (_locker)
        {
            if (_isSet)
                return;
                //throw new InvalidOperationException("ֻ�������һ��Id������");

            Task.Delay(new Random().Next(10, 100)).Wait();

            SetIdGenerator(idGeneratorConfig);

            _isSet = true;
        }
    }

    /// <summary>
    /// ����Id������
    /// </summary>
    /// <param name="idGeneratorConfig"></param>
    /// <exception cref="Exception"></exception>
    private static void SetIdGenerator(IdGeneratorConfig idGeneratorConfig)
    {
        var redisProvider = AppInfo.GetRequiredService<IRedisClient>(false);
        using var lockController = redisProvider.Lock($"{idGeneratorConfig.CachePrefix}:lock", 5);

        if (lockController == null)
        {
            Task.Delay(new Random().Next(100, 1000)).Wait();
            SetIdGenerator(idGeneratorConfig);
        }

        try
        {
            var hostName = ":host:";
            var cache = AppInfo.GetRequiredService<ICacheTool>(false);
            var keys = cache.GetKeysByPattern($"{idGeneratorConfig.CachePrefix}{hostName}*");

            var maxWorkerId = (short)(Math.Pow(2.0, idGeneratorConfig.WorkerIdBitLength) - 1);
            var workerIdList = new List<ushort>();
            for (ushort i = 0; i < maxWorkerId; i++)
            {
                workerIdList.Add(i);
            }

            foreach (var key in keys)
            {
                var workerId = key[(key.LastIndexOf(':') + 1)..];
                workerIdList.Remove(Convert.ToUInt16(workerId));
            }

            var workerIdKey = string.Empty;
            foreach (var workerId in workerIdList)
            {
                workerIdKey = $"{idGeneratorConfig.CachePrefix}{hostName}{AppInfo.HostInfo.ShortName}:{workerId}";
                var exists = cache.Exists(workerIdKey);
                if (exists)
                {
                    workerIdKey = string.Empty;
                    continue;
                }

                Console.WriteLine($"{Environment.NewLine}�Զ�ע��Ļ����� WorkerId = {workerId}");

                idGeneratorConfig.WorkerId = workerId;
                YitIdHelper.SetIdGenerator(idGeneratorConfig);

                cache.Set(workerIdKey, string.Empty, TimeSpan.FromSeconds(15));

                break;
            }

            if (workerIdKey.IsNull())
            {
                throw new Exception("�Զ�ע�������WorkerId��ȫ��ռ�ã������ӻ�����λ��WorkerIdBitLength������������");
            }

            //ÿ�� 10 ��ˢ�� WorkerId ռ����Ч��
            Task.Run(() =>
            {
                while (true)
                {
                    redisProvider.Expire(workerIdKey, TimeSpan.FromSeconds(15));
                    Task.Delay(10000).Wait();
                }
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        finally
        {
            lockController.Unlock();
        }
    }
}
