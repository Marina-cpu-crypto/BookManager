namespace WriterApp.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public List<Collection> Collections { get; set; }

        public User(string nickName, string email)
        {
            UserId = Guid.NewGuid();
            NickName = nickName;
            Email = email;
            Collections = new List<Collection>()
            {
                new Collection(0,"В процессе"),
                new Collection(1,"Завершено")
            };

        }
    }
}
