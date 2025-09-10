using Polly;

namespace ChipSys.Admin.Core.Helpers;

/// <summary>
/// ���԰�����
/// </summary>
public class PolicyHelper
{
    public static List<IAsyncPolicy<HttpResponseMessage>> GetPolicyList()
    {
        //�������
        //var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(10, 100);

        //���˲���
        //����Ҳ��Ϊ���񽵼�������ָ���ڷ�������ʱ�ı��÷�����
        //var fallbackPolicy = Policy<string>.Handle<HttpRequestException>().FallbackAsync("backup strategy");

        //�������
        //var cachePolicy = Policy.CacheAsync<HttpResponseMessage>(cacheProvider, TimeSpan.FromSeconds(60));

        //��ʱ����
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(9);

        // ���Բ���
        // ���ڳ�ʱ����Ӧ״̬��>=500�Ĵ����������3�Ρ�
        var retryPolicy = Policy.Handle<Exception>()
            .OrResult<HttpResponseMessage>(response =>
            {
                return (int)response.StatusCode >= 500;
            })
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(5)
            });

        //�۶ϲ���
        var circuitBreakerPolicy = Policy.Handle<Exception>()
            .CircuitBreakerAsync
            (
                // ���۶�ǰ������쳣����
                exceptionsAllowedBeforeBreaking: 2,
                // �۶ϳ���ʱ��
                durationOfBreak: TimeSpan.FromMinutes(10),
                // �۶ϴ����¼�
                onBreak: (ex, breakDelay) =>
                {
                    Console.WriteLine("�۶ϴ����¼�");
                },
                //�۶ϻָ��¼�
                onReset: () =>
                {
                    Console.WriteLine("�۶ϻָ��¼�");
                },
                //�۶Ͻ����¼�
                onHalfOpen: () =>
                {
                    Console.WriteLine("�۶Ͻ����¼�");
                }
            ).AsAsyncPolicy<HttpResponseMessage>();

        return new List<IAsyncPolicy<HttpResponseMessage>>()
        {
            retryPolicy,
            timeoutPolicy,
            circuitBreakerPolicy
        };
    }
}
