using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using OnceMi.AspNetCore.OSS;
using ChipSys.Admin.Core.Configs;
using OSSOptions = ChipSys.Admin.Core.Configs.OSSOptions;

namespace ChipSys.Admin.Core.Extensions;

/// <summary>
/// OSS��չ
/// </summary>
public static class OSSExtensions
{
    /// <summary>
    /// �����洢Ͱ
    /// </summary>
    /// <param name="oSSServiceFactory"></param>
    /// <param name="oSSOptions"></param>
    private static void CreateBucketName(IOSSServiceFactory oSSServiceFactory, OSSOptions oSSOptions)
    {
        var oSSService = oSSServiceFactory.Create(oSSOptions.Provider.ToString());
        if (!oSSService.BucketExistsAsync(oSSOptions.BucketName).Result)
        {
            oSSService.CreateBucketAsync(oSSOptions.BucketName).Wait();
        }

        //����Minio�洢ͰȨ��
        if (oSSOptions.Provider == OSSProvider.Minio)
        {
            var bucketName = oSSOptions.BucketName;
            var minioClient = new MinioClient()
                .WithEndpoint(oSSOptions.Endpoint)
                .WithCredentials(oSSOptions.AccessKey, oSSOptions.SecretKey);

            if (oSSOptions.Region.NotNull())
            {
                minioClient.WithRegion(oSSOptions.Region);
            }

            minioClient = minioClient.Build();
            //�鿴�洢ͰȨ��
            //var policy = minioClient.GetPolicyAsync(new GetPolicyArgs().WithBucket(bucketName)).Result;
            //���ô洢ͰȨ�ޣ��洢Ͱ�ڵ������ļ�����ͨ���������÷���
            var policy = $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Action"":[""s3:GetBucketLocation""],""Resource"":[""arn:aws:s3:::{bucketName}""]}},{{""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Action"":[""s3:GetObject""],""Resource"":[""arn:aws:s3:::{bucketName}/*.*""]}}]}}";
            var setPolicyArgs = new SetPolicyArgs().WithBucket(bucketName).WithPolicy(policy);
            minioClient.SetPolicyAsync(setPolicyArgs).Wait();
        }
    }

    /// <summary>
    /// ����OSS
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddOSS(this IServiceCollection services)
    {
        var oSSConfig = services.BuildServiceProvider().GetRequiredService<IOptions<OSSConfig>>().Value;

        if (oSSConfig.OSSConfigs != null && oSSConfig.OSSConfigs.Any(s => s.Enable))
        {
            foreach (var oSSOptions in oSSConfig.OSSConfigs)
            {
                if (oSSOptions.Enable)
                {
                    services.AddOSSService(oSSOptions.Provider.ToString(), option =>
                    {
                        option.Provider = oSSOptions.Provider;
                        option.Endpoint = oSSOptions.Endpoint;
                        option.Region = oSSOptions.Region;
                        option.AccessKey = oSSOptions.AccessKey;
                        option.SecretKey = oSSOptions.SecretKey;
                        option.IsEnableHttps = oSSOptions.IsEnableHttps;
                        option.IsEnableCache = oSSOptions.IsEnableCache;
                    });

                    var oSSServiceFactory = services.BuildServiceProvider().GetRequiredService<IOSSServiceFactory>();
                    CreateBucketName(oSSServiceFactory, oSSOptions);
                }
            }
        }
        else
        {
            //δ����OSSע��
            services.AddOSSService(option =>
            {
                option.Provider = OSSProvider.Invalid;
            });
        }

        return services;
    }
}
