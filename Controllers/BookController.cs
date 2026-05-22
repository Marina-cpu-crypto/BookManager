using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Documents.Data;
using Documents.Models;

namespace Documents.Controllers
{
    public class BookController : Controller
    {
        IBookRepository bookRepository;

        public BookController(IBookRepository bookRep)
        {
            this.bookRepository = bookRep;
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
            SaveBooksToFile();

            return RedirectToAction("Index", new { id = id });
        }

        public IActionResult Save(Guid Id, string Name, string Author, string Genre, string Description, bool IsRead, string? Review)
        {
            var book = bookRepository.TryGetById(Id);
            if (book == null)
                return NotFound();

            book.Name = Name;
            book.Author = Author;
            book.Genre = Genre;
            book.Description = Description;
            book.IsRead = IsRead;

            if (!string.IsNullOrEmpty(Review))
                book.Review = Review;

            SaveBooksToFile();

            return RedirectToAction("Index", new { id = Id });
        }

        public IActionResult Delete(Guid Id)
        {
            var books = bookRepository.GetAll();
            var bookToRemove = books.FirstOrDefault(b => b.Id == Id);

            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                SaveBooksToFile();
            }

            return RedirectToAction("Index", "Home");
        }

        private void SaveBooksToFile()
        {
            var books = bookRepository.GetAll();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(books, options);
            System.IO.File.WriteAllText("Data/books.json", json);
        }
    }
}
