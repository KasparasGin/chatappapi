using System.Xml.Serialization;
using chatappapi.Data;
using chatappapi.Models;
using Microsoft.EntityFrameworkCore;

namespace chatappapi.Services
{
    public class MessageService : IMessageService
    {
        ChatAppContext _context;

        public MessageService(ChatAppContext context)
        {
            _context = context;
        }

        public async Task SaveMessage(int receiver, int sender, string text)
        {
            var message = new Message { ReceiverId = receiver, SenderId = sender, Text = text, Timestamp = DateTime.Now };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            
        }
    }
    public interface IMessageService
    {
        public Task SaveMessage(int receiver, int sender, string text);
    }

}
