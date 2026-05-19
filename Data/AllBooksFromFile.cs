using Microsoft.Extensions.Options;
using System.Text.Json;
using Documents.Data;
using Documents.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Documents.Data
{
    public class AllBooksFromFile : IBookRepository
    {
        private static List<Book> books = new List<Book>();
        //private static List<Collection> collections;

        public AllBooksFromFile(ICollectionsRepository collectRep)
        {
            //collections = collectRep.GetAll();
            string jsonString = File.ReadAllText("Data/books.json");
            books = JsonSerializer.Deserialize<List<Book>>(jsonString);
        }

        public List<Book> GetAll()
        {
            return books;
        }

        public Book? TryGetById(Guid id)
        {
            return books.FirstOrDefault(book => book.Id == id);
        }
        
    }
}
