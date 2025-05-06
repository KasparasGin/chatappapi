using chatappapi.Controllers;

namespace chatappapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSignalR();
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll", builder => {
                    builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.MapHub<ChatHub>("/chathub");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
