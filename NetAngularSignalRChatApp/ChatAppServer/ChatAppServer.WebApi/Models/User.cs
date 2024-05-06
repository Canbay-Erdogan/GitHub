namespace ChatAppServer.WebApi.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
        }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; }= string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
