namespace chatappapi.Controllers;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string toUserId, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", toUserId, message);

    }
}