using EmirhanSignalR.Web.Models;

namespace EmirhanSignalR.Web.Hubs
{
    public interface IExampleTypeSafeHub
    {
        Task ReceiveMessageForallClient(string message);
        Task ReceiveMessageAsStreamForallClient(string name);
        Task ReceiveProductAsStreamForallClient(Product product);
        Task ReceiveTypedMessageForallClient(Product product);
        Task ReceiveConnectedClientCountAllClient(int clientCount);
        Task ReceiveMessageForCallerClient(string message);
        Task ReceiveMessageForOthersClient(string message);
        Task ReceiveMessageForIndividualClient(string message);
        Task ReceiveMessageForGroupClients(string message);

    }
}
