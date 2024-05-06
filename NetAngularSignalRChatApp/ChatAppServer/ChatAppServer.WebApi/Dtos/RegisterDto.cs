namespace ChatAppServer.WebApi.Dtos
{
    public sealed record class RegisterDto
        (
        string name,
        IFormFile File
        );
   
}
