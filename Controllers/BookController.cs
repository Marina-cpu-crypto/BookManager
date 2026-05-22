using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Documents.Data;
using Documents.Models;

namespace Documents.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly ICollectionsRepository collectionsRepository;

        public BookController(IBookRepository bookRep, ICollectionsRepository collectRep)
        {
            this.bookRepository = bookRep;
            this.collectionsRepository = collectRep;
        }

        public IActionResult Index(Guid id)
        {
            var book = bookRepository.TryGetById(id);
            if (book == null)
                return NotFound();
            
            return View(book);
        }

        public IActionResult Redact(Guid id)
        {
            var book = bookRepository.TryGetById(id);
            if (book == null)
                return NotFound();
            
            return View(book);
        }

        public IActionResult ChangeReadStatus(Guid id)
        {
            var book = bookRepository.TryGetById(id);
            if (book == null)
                return NotFound();

            book.IsRead = !book.IsRead;

            var books = bookRepository.GetAll();
            var collections = collectionsRepository.GetAll();
            SaveToFile(books, collections);
            
            return RedirectToAction("Index", new { id = id });
        }

        public IActionResult Save(Guid Id, string Name, string Author, string Genre, string Description, bool IsRead, string? Review)
        {
            var book = bookRepository.TryGetById(Id);
            if (book == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Author))
                return BadRequest("Название и автор обязательны");

            book.Name = Name;
            book.Author = Author;
            book.Genre = Genre;
            book.Description = Description;
            book.IsRead = IsRead;
            
            if (!string.IsNullOrEmpty(Review))
                book.Review = Review;

            var books = bookRepository.GetAll();
            var collections = collectionsRepository.GetAll();
            SaveToFile(books, collections);

            return RedirectToAction("Index", new { id = Id });
        }

        public IActionResult Delete(Guid Id)
        {
            var books = bookRepository.GetAll();
            var bookToRemove = books.FirstOrDefault(b => b.Id == Id);
            
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                var collections = collectionsRepository.GetAll();
                SaveToFile(books, collections);
            }

            return RedirectToAction("Index", "Home");
        }

        private void SaveToFile(List<Book> books, List<Collection> collections)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string booksJson = JsonSerializer.Serialize(books, options);
            string collectionsJson = JsonSerializer.Serialize(collections, options);
            
            System.IO.File.WriteAllText("Data/books.json", booksJson);
            System.IO.File.WriteAllText("Data/collections.json", collectionsJson);
        }
    }
}
