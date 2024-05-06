namespace ChatAppServer.WebApi.Dtos
{
    public sealed record SendMessageDto (
        Guid userId,
        Guid ToUserId,
        string Message
        );
}
