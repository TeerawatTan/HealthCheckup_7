using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HelpCheck_Web.Hubs
{
    public class MyHub: Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage","1");
        }
    }
}
