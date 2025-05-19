using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using YallaR7la.Services;
using YallaR7la.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

public class EmailServices : IEmailServices
{
    private readonly EmailSetings _services;

    public EmailServices(IOptions<EmailSetings> services)
    {
        _services = services.Value;
    }
    public async Task SendEmailAsync(string emailTo, string subject, string body, IList<IFormFile> attachments = null)
    {
        var email = new MimeMessage
        { 
            Sender = MailboxAddress.Parse(_services.Email),
            Subject = subject,
        };

        email.To.Add(MailboxAddress.Parse(emailTo));

        var builder = new BodyBuilder();
         if (attachments != null)
        {
            byte[] fileBytes;

            foreach (var attachment in attachments)
            {
                if (attachment.Length > 0)
                {
                    using var ms = new MemoryStream();
                    attachment.CopyTo(ms);
                    fileBytes = ms.ToArray();
                    builder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }
        }
         builder.HtmlBody = body;
        email.Body =  builder.ToMessageBody();
        email.From.Add(new MailboxAddress(_services.DisplayName , _services.Email));

        using var smtp = new SmtpClient();
        smtp.Connect(_services.Host, _services.Port, SecureSocketOptions.StartTls);

        smtp.Authenticate(_services.Email , _services.Password);
        await smtp.SendAsync(email);

        smtp.Disconnect(true);
                
    }
}
