namespace Documents.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        //public UsersData UserData { get; set; }

        public User(string nickName, string email)
        {
            UserId = Guid.NewGuid(); 
            NickName = nickName;
            Email = email;
        }
    }
    public class UsersData
    {
        public UsersData(Guid dataId)
        {
            DataId = dataId;
        }

        public List<Collection> Collections { get; set; } = new List<Collection>() 
        {
            new Collection(0, "В процессе"),
            new Collection(1, "Завершено")
        };
        public Guid DataId { get; }
    }
}

