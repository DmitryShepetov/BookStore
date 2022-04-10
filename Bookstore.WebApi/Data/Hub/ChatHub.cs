using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Bookstore.WebApi
{
    public class ChatHub : Hub
    {
        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveOne", user, message);
        }
    }
}
