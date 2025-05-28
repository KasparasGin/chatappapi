namespace chatappapi.Controllers;

using chatappapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class ChatHub : Hub
{
    private IMessageService _messageService;

    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task SendMessage(int toUserId, string message)
    {
        await _messageService.SaveMessage(toUserId, Int32.Parse(Context.UserIdentifier), message);
        await Clients.User(toUserId.ToString()).SendAsync("ReceiveMessage", toUserId.ToString(), message);
    }
}