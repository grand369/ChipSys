using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DotNetCore.CAP;
using MimeKit;
using MailKit.Net.Smtp;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Services.Email.Events;
using ChipSys.Admin.Resources;

namespace ChipSys.Admin.Services.Email;

public class EmailService: ICapSubscribe
{
    private readonly EmailConfig _emailConfig;
    private readonly AdminLocalizer _adminLocalizer;

    public EmailService(IOptions<EmailConfig> emailConfig, AdminLocalizer adminLocalizer)
    {
        _emailConfig = emailConfig.Value;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// �ʼ�����
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    [NonAction]
    [CapSubscribe(SubscribeNames.EmailSingleSend)]
    public async Task SingleSendAsync(EmailSingleSendEvent @event)
    {
        var emailConfig = _emailConfig;

        var builder = new BodyBuilder()
        {
            HtmlBody = @event.Body
        };
        var message = new MimeMessage()
        {
            Subject = @event.Subject,
            Body = builder.ToMessageBody()
        };

        var fromEmailName = @event.FromEmail!=null && @event.FromEmail.Name.NotNull() ? @event.FromEmail.Name : emailConfig.FromEmail.Name;
        var fromEmailAddress = @event.FromEmail != null && @event.FromEmail.Address.NotNull() ? @event.FromEmail.Address : emailConfig.FromEmail.Address;
        message.From.Add(new MailboxAddress(fromEmailName, fromEmailAddress));
        message.To.Add(new MailboxAddress(@event.ToEmail.Name, @event.ToEmail.Address));

        using var client = new SmtpClient();
        await client.ConnectAsync(emailConfig.Host, emailConfig.Port, emailConfig.UseSsl);
        // ����Ƿ���Ҫ�����֤
        var hasAuthentication = client.Capabilities.HasFlag(SmtpCapabilities.Authentication);
        if (hasAuthentication)
        {
            await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
        }
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    /// <summary>
    /// ����������֤��
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    [NonAction]
    [CapSubscribe(SubscribeNames.EmailSendCode)]
    public async Task SendCodeAsync(EmailSendCodeEvent @event)
    {
        await SingleSendAsync(new EmailSingleSendEvent
        {
            ToEmail = new EmailSingleSendEvent.Models.EmailModel
            {
                Address = @event.ToEmail.Address,
            },
            Subject = _adminLocalizer["������֤��"],
            Body = _adminLocalizer["<p>�����ڽ��������¼����</p><p>������֤��: {0}����Ч��5����</p><p>��Ǳ��˲���������ԡ�</p>", @event.Code]
        });
    }
}
