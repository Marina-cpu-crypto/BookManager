using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Documents.Data;
using Documents.Models;

namespace Documents.Controllers
{
    public class BookController : Controller
    {
        IBookRepository bookRepository;
        ICollectionsRepository collectionsRepository;

        public Guid MainId = Guid.Parse(System.IO.File.ReadAllText("Data/MainId.txt"));
        public BookController(IBookRepository bookRep, ICollectionsRepository collRep)
        {
            this.collectionsRepository = collRep;   
            this.bookRepository = bookRep;
        }


        public IActionResult Index(Guid id)
        {
            var book = collectionsRepository.TryGetById(id);
            return View(book);
        }
        
        public IActionResult Redact(Guid id)
        {
            var book = collectionsRepository.TryGetById(id);
            return View(book);
        }

        public IActionResult ChangeStatus(Guid id)
        {
            bookRepository.ChangeStatus(id);
            
            return RedirectToAction("Index", new { id = id });
        }

        public IActionResult SetRating(Guid id, int rating)
        {
            var book = collectionsRepository.TryGetById(id);
            book.Rating = rating;

            bookRepository.Change(book);
            bookRepository.Resave();

            return RedirectToAction("Index", new { id = id });
        }

        //public IActionResult ChangeImage(Guid id, string PathImage)
        //{
        //    var book = collectionsRepository.TryGetById(id);
        //    book.PathImage = PathImage;
        //    bookRepository.Change(book);

        //    return RedirectToAction("Index","Book" ,new { id = id });
        //}

        public IActionResult Save(Guid Id, string Name, string Author, string Genre, string bookText, string Description, string PathImage)
        {
            Book book = collectionsRepository.TryGetById(Id);

            if(book.Name != Name)
            {

                string oldFile = "Data/" + MainId + "/Texts/" + book.Name + ".txt";
                if(System.IO.File.Exists(oldFile))    // добавление проверки
                    System.IO.File.Delete(oldFile);
                book.Name = Name;
            }
            book.Author = Author;
            book.Genre = Genre;
            book.Description = Description;
            book.PathImage = PathImage;

            string file = "Data/" + MainId + "/Texts/" + Name + ".txt";
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(file)); // исправило баг (?)
            if (!string.IsNullOrEmpty(bookText)) // проверка на нулл
                System.IO.File.WriteAllText(file, bookText);

            bookRepository.Change(book);

            return RedirectToAction("Index", new { id = Id });
        }

        public IActionResult Delete(Guid Id)
        {
            bookRepository.Delete(Id);

            return RedirectToAction("Index", "Home");
        }
    }
}
