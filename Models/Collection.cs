namespace Documents.Models
{
    public class Collection
    {
        public Guid CollectionId { get;}
        public string Name { get; set; }
        // public int Amount { get; set; } = 0; наверно ненужный класс
        public Dictionary<Guid,string> Books { get; set; } = new Dictionary<Guid,string>();

        public Collection(string name)
        {
            CollectionId = Guid.NewGuid();
            Name = name;
        }

    }
}
