using ChatAppServer.WebApi.Context;
using ChatAppServer.WebApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.WebApi.Hubs
{
    public  class ChatHub(ApplicationDbContext context):Hub
    {
        public static Dictionary<string, Guid> Users = new();
        public async Task Connect(Guid userId)
        {
            Users.Add(Context.ConnectionId, userId);
            User? user = await context.Users.FindAsync(userId);
            if (user is not null)
            {
                user.Status = "online";
                await context.SaveChangesAsync();

                await Clients.All.SendAsync("Users", user);
            }
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            Guid userId;
            Users.TryGetValue(Context.ConnectionId, out userId);
            Users.Remove(Context.ConnectionId);
           
            User? user = await context.Users.FindAsync(userId);
            if (user is not null)
            {
                user.Status = "offline";
                await context.SaveChangesAsync();

                await Clients.All.SendAsync("Users", user);
            }
        }
    }
}
