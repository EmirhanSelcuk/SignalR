using EmirhanSignalR.Web.Models;
using Microsoft.AspNetCore.SignalR;

namespace EmirhanSignalR.Web.Hubs
{
    public class ExampleTypeSafeHub:Hub<IExampleTypeSafeHub>
    {
        private static int ConnectedClientCount = 0;

        public async Task BroadcastMessageToAllClient(string message)
        {
            await Clients.All.ReceiveMessageForallClient(message);
        }

        public async Task BroadcastTypedMessageToAllClient(Product product)
        {
            await Clients.All.ReceiveTypedMessageForallClient(product);
        }
        public async Task BroadcastMessageToCallerClient(string message)
        {
            await Clients.Caller.ReceiveMessageForCallerClient(message);
        }

        public async Task BroadcastMessageToOthersClient(string message)
        {
            await Clients.Others.ReceiveMessageForOthersClient(message);
        }

        public async Task BroadcastMessageToIndividualClient(string connectionId,string message)
        {
            await Clients.Client(connectionId).ReceiveMessageForIndividualClient(message);
        }

        public async Task BroadcastStreamDataToAllClient(IAsyncEnumerable<string> nameAsChunks)
        {

            await foreach (var name in nameAsChunks)
            {
                await Task.Delay(1000);
                await Clients.All.ReceiveMessageAsStreamForallClient(name);
            }
        }
        public async Task BroadcastStreamProductToAllClient(IAsyncEnumerable<Product> productAsChunks)
        {

            await foreach (var product in productAsChunks)
            {
                await Task.Delay(1000);
                await Clients.All.ReceiveProductAsStreamForallClient(product);
            }
        }

        public async IAsyncEnumerable<string> BroadcastFromHubToClient(int count)
        {
            foreach(var item in Enumerable.Range(1, count).ToList())
            {
                await Task.Delay(1000);
                yield return $"{item}.data";
            }
        }


        public async Task BroadcastMessageToGroupClient(string groupName,string message)
        {
            await Clients.Group(groupName).ReceiveMessageForGroupClients(message);
        }

        public async Task AddGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.ReceiveMessageForCallerClient($"{groupName} grubuna eklendiniz.");

            //await Clients.Others.ReceiveMessageForOthersClient($"Kullanıcı({Context.ConnectionId}) {groupName} dahil oldu");

            await Clients.Group(groupName).ReceiveMessageForGroupClients($"Kullanıcı({Context.ConnectionId}) {groupName} dahil oldu");


        }
        public async Task RemoveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Caller.ReceiveMessageForCallerClient($"{groupName} grubundan çıktınız.");

            //await Clients.Others.ReceiveMessageForOthersClient($"Kullanıcı({Context.ConnectionId}) {groupName} gruptan çıktı");

            await Clients.Group(groupName).ReceiveMessageForGroupClients($"Kullanıcı({Context.ConnectionId}) {groupName} grubundan çıktı");
        }
        public override async Task OnConnectedAsync()
        {
            ConnectedClientCount++;

            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await base.OnConnectedAsync();
           
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientCount--;
            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await base.OnDisconnectedAsync(exception);
        }


    }

    

}
