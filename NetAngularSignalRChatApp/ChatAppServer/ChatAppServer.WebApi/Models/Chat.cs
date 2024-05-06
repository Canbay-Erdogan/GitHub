namespace ChatAppServer.WebApi.Models
{
    public sealed class Chat
    {
        public Guid Id { get; set; }

        public Chat()
        {
            Id = Guid.NewGuid();
        }

        public Guid UserId { get; set; }
        public Guid ToUserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }

    }
}
