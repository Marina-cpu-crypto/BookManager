namespace Documents.Models
{
    public class User
    {
        public Guid UserId { get; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserImage { get; set; } = string.Empty;

        public User(string name, string password, string email)
        {
            UserId = Guid.NewGuid();
            Name = name;
            Password = password;
            Email = email;
        }


    }
}