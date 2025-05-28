using Humanizer;

namespace chatappapi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
