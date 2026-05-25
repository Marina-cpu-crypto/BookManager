namespace WriterApp.Models
{
    public class Collection
    {
        public Collection(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Collection() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; } = 0;
        //public Dictionary<Guid,string> Books { get; set; } = new Dictionary<Guid,string>();
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
