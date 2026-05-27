namespace Documents.Models
{
    public class UserData
    {
        public UserData(Guid dataId)
        {
            DataId = dataId;
        }

        public Guid DataId { get;}
        public List<Collection> Collections { get; set; } = new List<Collection>()
        {
            new Collection(0, "В процессе"),
            new Collection(1, "Завершено")
        };
    }
}

