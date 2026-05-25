using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Documents.Data;
using Documents.Models;

namespace Documents.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICollectionsRepository collectionsRepository;
        private readonly IBookRepository bookRepository;

        public HomeController(ICollectionsRepository collectRep, IBookRepository bookRep)
        {
            this.collectionsRepository = collectRep;
            this.bookRepository = bookRep;
        }

        public IActionResult Index()
        {
            var collections = collectionsRepository.GetAll();
            return View(collections);
        }

        public IActionResult AddNew(string Name, string Author, string Genre, string Description, bool IsRead, string? Review)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Author))
            {
                return BadRequest("Название и автор обязательны");
            }

            Book book = new Book(Name, Author, Genre, Description);
            
            if (!string.IsNullOrEmpty(Review))
                book.Review = Review;

            book.IsRead = IsRead;
            
            var books = bookRepository.GetAll();
            books.Add(book);

            // Добавляем книгу в соответствующую коллекцию
            var collections = collectionsRepository.GetAll();
            if (IsRead && collections.Count > 1)
            {
                collections[1].Books.Add(book.Id, book.Name);
            }
            else if (!IsRead && collections.Count > 0)
            {
                collections[0].Books.Add(book.Id, book.Name);
            }

            SaveToFile(books, collections);

            return RedirectToAction("Index");
        }

        private void SaveToFile(List<Book> books, List<Collection> collections)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            string newbook = JsonSerializer.Serialize(books, options);
            string newcoll = JsonSerializer.Serialize(collections, options);
            
            System.IO.File.WriteAllText("Data/books.json", newbook);
            System.IO.File.WriteAllText("Data/collections.json", newcoll);
        }
    }
}
