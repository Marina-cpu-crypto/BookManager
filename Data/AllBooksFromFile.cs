using Microsoft.Extensions.Options;
using System.Text.Json;
using Documents.Data;
using Documents.Models;
using Documents.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Documents.Data
{
    public class AllBooksFromFile : IBookRepository
    {
        // поиск по абсолютному пути
        ICollectionsRepository collectionsRepository;
        private static readonly string DataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");

        public Guid MainId = Guid.Parse(File.ReadAllText(Path.Combine(DataPath, "Data/MainId.txt")));

        private static List<Collection> collections;

        public AllBooksFromFile(ICollectionsRepository collectRep)
        {
            this.collectionsRepository = collectRep;
            collections = collectRep.GetOne(MainId);
            string jsonPath = Path.Combine(DataPath, "Data", MainId.ToString(), "books.json");
            
            // Проверка существования файла букс
            if (!File.Exists(jsonPath))
            {
                var directory = Path.GetDirectoryName(jsonPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                
                var options = new JsonSerializerOptions { WriteIndented = true };
                var emptyBooks = new List<Book>();
                string emptyJson = System.Text.Json.JsonSerializer.Serialize(emptyBooks, options);
                File.WriteAllText(jsonPath, emptyJson);
            }
            
            string jsonString = File.ReadAllText(jsonPath);

        }
        public void Resave() { collectionsRepository.ResaveUserData(collections); }

        public void ChangeStatus(Guid id)
        {
            var book = collectionsRepository.TryGetById(id);
            book.IsDone = !book.IsDone;
            if (book.IsDone)
            {
                collections[1].Books.Add(book);
                this.RemoveBookFromCollection(book, 0);

                collections[1].Amount++;
                collections[0].Amount--;
            }
            else
            {
                collections[0].Books.Add(book);
                this.RemoveBookFromCollection(book, 1);

                collections[0].Amount++;
                collections[1].Amount--;
            }
            this.Resave();
        }

        public void Change(Book book)
        {
            int ind = 0;
            if (book.IsDone) ind = 1;

            this.RemoveBookFromCollection(book, ind);
            collections[ind].Books.Add(book);

            this.Resave();
        }

        public void Delete(Guid id)
        {
            this.MainId = Guid.Parse(File.ReadAllText("Data/MainId.txt"));
            var book = collectionsRepository.TryGetById(id);
            string filePath = "Data/" + MainId + "/Texts/" + book.Name + ".txt";
            if (System.IO.File.Exists(filePath)) // добавление проверки перед удалением
            {
                System.IO.File.Delete(filePath);
            }

            if (book.IsDone) RemoveBookFromCollection(book, 1);
            else this.RemoveBookFromCollection(book, 0);

            this.Resave();

        }
        public void RemoveBookFromCollection(Book book, int ind)
        {
            this.MainId = Guid.Parse(File.ReadAllText("Data/MainId.txt"));
            collections = collectionsRepository.GetOne(MainId);
            for (int i = 0; i < collections[ind].Books.Count; i++)
            {
                if (collections[ind].Books[i].Id == book.Id)
                {
                    collections[ind].Books.RemoveAt(i);
                    break;
                }
            }
        }



        //public void Resave()
        //{
        //    var options = new JsonSerializerOptions { WriteIndented = true }; // добавляет отступы и переносы строк

        //    string newbooks = JsonSerializer.Serialize(books, options);
        //    File.WriteAllText("Data/" + MainId + "/books.json", newbooks);


        //    this.ResaveUserData();
        //    //this.Sort();
        //    //string newcol = JsonSerializer.Serialize(collections, options);
        //    //File.WriteAllText("Data/collections.json", newcol);
        //}




        //public void Sort()
        //{
        //    collections[0].Books.OrderByDescending(b => b.Rating);
        //    collections[1].Books.OrderByDescending(b => b.Rating);
        //}

    }
}
