namespace Documents.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; } //Жанр
        public string Description { get; set; }
        //public DateOnly Date { get; set; } =???   //Дата (чего? Написания, добавления?)
        public bool IsRead{ get; set; } = false; //Прочтена или нет
        public string Review? { get; set; } ; //Пользовательские заметки, рецензия

        public Book(string name, string author, string genre, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Author = author;
            Genre = genre;
            Description = description;
        }

    }
}
