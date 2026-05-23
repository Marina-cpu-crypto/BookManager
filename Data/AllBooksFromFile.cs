using System.Text.Json;
using Documents.Models;

namespace Documents.Data
{
    public class AllBooksFromFile : IBookRepository
    {
        private const string BooksFilePath = "Data/books.json";

        public List<Book> GetAll()
        {
            try
            {
                if (!File.Exists(BooksFilePath))
                    return new List<Book>();

                string jsonString = File.ReadAllText(BooksFilePath);
                
                if (string.IsNullOrWhiteSpace(jsonString))
                    return new List<Book>();

                var books = JsonSerializer.Deserialize<List<Book>>(jsonString);
                return books ?? new List<Book>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке книг: {ex.Message}");
                return new List<Book>();
            }
        }

        public Book? TryGetById(Guid id)
        {
            var books = GetAll();
            return books.FirstOrDefault(book => book.Id == id);
        }
    }
}
