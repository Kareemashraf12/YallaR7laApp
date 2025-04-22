using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Your App Name", _configuration["MailSettings:SenderEmail"]));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain") { Text = message };

        using var client = new SmtpClient();
        await client.ConnectAsync(_configuration["MailSettings:SmtpServer"], int.Parse(_configuration["MailSettings:Port"]), SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_configuration["MailSettings:Username"], _configuration["MailSettings:Password"]);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}
