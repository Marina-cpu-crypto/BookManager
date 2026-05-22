using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Documents.Data;
using Documents.Models;

namespace Documents.Controllers
{
    public class HomeController : Controller
    {
        ICollectionsRepository collectionsRepository;
        IBookRepository bookRepository;
        List<Collection> collections;
        List<Book> books;

        public HomeController(ICollectionsRepository collectRep, IBookRepository bookRep)
        {
            this.collectionsRepository = collectRep;
            this.bookRepository = bookRep;
            collections = collectionsRepository.GetAll();
            books = bookRepository.GetAll();
        }

        public IActionResult Index()
        {
            return View(collections);
        }

        public IActionResult AddNew(string Name, string Author, string Genre, string Description, bool IsRead, string? Review)
        {
            Book book = new Book(Name, Author, Genre, Description);

            if (!string.IsNullOrEmpty(Review))
                book.Review = Review;

            book.IsRead = IsRead;

            books.Add(book);

            if (IsRead)
            {
                if (collections.Count > 1)
                {
                    collections[1].Books.Add(book.Id, book.Name);
                }
            }
            else
            {
                if (collections.Count > 0)
                {
                    collections[0].Books.Add(book.Id, book.Name);
                }
            }

            var options = new JsonSerializerOptions { WriteIndented = true };

            string newbook = JsonSerializer.Serialize(books, options);
            string newcoll = JsonSerializer.Serialize(collections, options);

            System.IO.File.WriteAllText("Data/books.json", newbook);
            System.IO.File.WriteAllText("Data/collections.json", newcoll);

            return RedirectToAction("Index");
        }
    }
}
