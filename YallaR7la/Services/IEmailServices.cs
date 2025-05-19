namespace YallaR7la.Services
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string emailTo , string subject , string body , IList<IFormFile> attachments = null);
    }
}
