using Microsoft.AspNetCore.SignalR;

namespace EmirhanSignalR.Web.Hubs
{
    public class ExampleHub:Hub
    {
        public async Task BroadcastMessageToAllClient(string message)
        {
            await Clients.All.SendAsync("ReceiveMessageForallClient", message);
        }
    }
}
