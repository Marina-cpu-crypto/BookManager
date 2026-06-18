namespace Documents.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public bool IsDone { get; set; }
        public int Rating { get; set; } = 0;
        public string Description { get; set; } = "";
        public string? PathImage { get; set; } = "";

        //public int[] rating {  get; set; } = new int[5] {1,2,3,4,5};
        //public int? PageNumber { get; set; }



        //public List<List<string>>? Matherials { get; set; }

        //public string bookText { get; set; } = "";

        public Book(string name, string author, string genre, bool isdone, string description = ""/*, List<List<string>>? matherials*/)
        {
            Id = Guid.NewGuid();

            Name = name;
            Author = author;
            Genre = genre;
            IsDone = isdone;
            Description = description;

            //PathImage = pathImage;
            //Matherials = matherials;
        }
    }
}
