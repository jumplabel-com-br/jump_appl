using System.Threading.Tasks;
namespace CoreUI.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}