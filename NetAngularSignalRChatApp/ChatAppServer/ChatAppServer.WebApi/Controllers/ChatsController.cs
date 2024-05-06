using ChatAppServer.WebApi.Context;
using ChatAppServer.WebApi.Dtos;
using ChatAppServer.WebApi.Hubs;
using ChatAppServer.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class ChatsController(ApplicationDbContext context, IHubContext<ChatHub> hubContext ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetChats(Guid userId, Guid toUserId, CancellationToken cancellationToken)
        {
            List<Chat> chats = await context.Chats.Where
                (c => c.UserId == userId && c.UserId == toUserId ||
                 c.ToUserId == userId && c.UserId == toUserId)
                .OrderBy(c => c.Date).ToListAsync(cancellationToken);

            return Ok(chats);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await context.Users.OrderBy(p=>p.Name).ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage (SendMessageDto request, CancellationToken cancellationToken)
        {
            Chat chat = new()
            {
                UserId = request.userId,
                ToUserId = request.ToUserId,
                Message = request.Message,
                Date = DateTime.Now
            };

            await context.Chats.AddAsync(chat,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            string conId = ChatHub.Users.First(p => p.Value == chat.ToUserId).Key;

            await hubContext.Clients.Client(conId).SendAsync("Messages",chat);
            return Ok(chat);
        }
    }
}
