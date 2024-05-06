using ChatAppServer.WebApi.Context;
using ChatAppServer.WebApi.Dtos;
using ChatAppServer.WebApi.Models;
using GenericFileService.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class AuthController (ApplicationDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDto request, CancellationToken cancellationToken)
        {
            bool isNameExist = await context.Users.AnyAsync(p=>p.Name == request.name , cancellationToken);

            if (isNameExist)
            {
                return BadRequest(new {Message = "Bu kullanıcı adı daha önce kullanılmış"});
            }

            string avatar = FileService.FileSaveToServer(request.File, "wwwroot/avatar/");

            User user = new()
            {
                Name = request.name,
                Avatar = avatar
            };
            await context.Users.AddAsync(user,cancellationToken);
            await context.SaveChangesAsync();

            return Ok(user);
        }


        [HttpGet]
        public async Task<IActionResult> Login(string name, CancellationToken cancellationToken)
        {
            User? user = await context.Users.FirstOrDefaultAsync(p=>p.Name == name ,cancellationToken);

            if(user == null)
            {
                return BadRequest(new { message = "Kullanıcı Bulunamadı" });
            }

            user.Status = "online";
            await context.SaveChangesAsync(cancellationToken);
            return Ok(user);
        }
    }
}
