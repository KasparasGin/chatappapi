namespace chatappapi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendMessage(string toUserId, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", toUserId, message);

    }
}