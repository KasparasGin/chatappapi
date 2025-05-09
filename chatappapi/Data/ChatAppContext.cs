using chatappapi.Models;
using Microsoft.EntityFrameworkCore;

namespace chatappapi.Data
{
    public class ChatAppContext : DbContext
    {
        public ChatAppContext(DbContextOptions<ChatAppContext> options): base(options) {    }

        public DbSet<User> Users { get; set; }
    }
}
