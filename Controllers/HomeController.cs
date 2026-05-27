using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using Documents.Data;
using Documents.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Documents.Controllers
{
    public class HomeController : Controller
    {
        public Guid MainId = Guid.Parse(System.IO.File.ReadAllText("Data/MainId.txt"));
        ICollectionsRepository collectionsRepository;
        IBookRepository bookRepository;
        List<Collection> collections;
        List<Book> books;
        public HomeController(ICollectionsRepository collectRep, IBookRepository bookRep)
        {
            this.collectionsRepository = collectRep;
            this.bookRepository = bookRep;

            collections = collectionsRepository.GetOne(MainId);
            books = bookRepository.GetAll();

        }

        public IActionResult Index()
        {
            //bookRepository.Sort();
            //collectionsRepository.ResetCollection();

            return View(collections);
        }
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult ViewAll()
        {
            return RedirectToAction("Index");
        }
        public IActionResult AddNew(string Name, string Author, string Genre, bool IsDone, string? Description, string? PathImage)
        {
            Book book = new Book(Name,Author, Genre, IsDone, Description ?? "");
            if(!string.IsNullOrEmpty(PathImage))  book.PathImage= PathImage;
            
            books.Add(book);
            if (IsDone)
            {
                collections[1].Books.Add(book);
                collections[1].Amount++;
            }
            else
            {
                collections[0].Books.Add(book);
                collections[0].Amount++;
            }

            var options = new JsonSerializerOptions { WriteIndented = true };

            string newbook = JsonSerializer.Serialize(books, options);
            //string newcoll = JsonSerializer.Serialize(collections, options);
            System.IO.File.WriteAllText("Data/books.json", newbook);
            //System.IO.File.WriteAllText("Data/collections.json", newcoll);
            collectionsRepository.ResaveUserData(collections);

            return RedirectToAction("Index");
        }

        public IActionResult Search(string Name, string Author, string Genre, string Description, int from, int to)
        {
            if ((from==0 && to==0) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Author) && string.IsNullOrEmpty(Genre) && string.IsNullOrEmpty(Description)) return RedirectToAction("Index");
            else
            {
                List<Collection> newcoll = new List<Collection>()
                {
                    new Collection(0,"\u0412 \u043F\u0440\u043E\u0446\u0435\u0441\u0441\u0435"),
                    new Collection(1,"\u0417\u0430\u0432\u0435\u0440\u0448\u0435\u043D\u043E")
                };
                foreach (var c in collections)
                {
                    foreach (var b in c.Books)
                    {
                        bool flag = false;
                        if (!string.IsNullOrEmpty(Name)) if (b.Name.Contains(Name, StringComparison.OrdinalIgnoreCase)) flag = true;
                        if (!string.IsNullOrEmpty(Author)) if (b.Author.Contains(Author, StringComparison.OrdinalIgnoreCase)) flag = true;
                        if (!string.IsNullOrEmpty(Genre)) if (b.Genre.Contains(Genre, StringComparison.OrdinalIgnoreCase)) flag = true;
                        if (!string.IsNullOrEmpty(Description)) if (b.Description.Contains(Description, StringComparison.OrdinalIgnoreCase)) flag = true;
                        if (from <= b.Rating && b.Rating <= to) flag = true;

                        if (flag)newcoll[c.Id].Books.Add(b);
                    }
                }
                return View(newcoll);
            }
        }

    }
}
