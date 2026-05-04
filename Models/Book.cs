namespace Documents.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Jenre { get; set; } //Жанр
        public string Description { get; set; }
        //public DateOnly Date { get; set; } =???   //Дата (чего? Написания, добавления?)
        public bool IsReadOrNot { get; set; } = false; //Прочтена или нет
        public string Reviuew { get; set; } = ""; //Пользовательские заметки, рецензия

        public Book(string name, string author, string jenre, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Author = author;
            Jenre = jenre;
            Description = description;
        }

    }
}
