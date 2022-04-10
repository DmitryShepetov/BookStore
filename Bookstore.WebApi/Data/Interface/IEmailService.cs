using Bookstore.WebApi.ViewModel;
using System.Threading.Tasks;

namespace Bookstore.WebApi.Data.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(MessageEmail message);
    }
}
