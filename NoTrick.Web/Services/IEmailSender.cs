using System.Net.Mail;
using System.Threading.Tasks;

namespace NoTrick.Web.Services {
    public interface IEmailSender {
        void SendEmail(MailMessage message);
        Task SendEmailAsync(MailAddress message);
    }
}