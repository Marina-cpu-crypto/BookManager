using Microsoft.Extensions.Options;
using System.Text.Json;
using WriterApp.Data;
using WriterApp.Models;
using WriterApp.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WriterApp.Data
{
    public class AllBooksFromFile : IBookRepository
    {
        ICollectionsRepository collectionsRepository;
        public Guid MainId = Guid.Parse(File.ReadAllText("Data/MainId.txt"));

        private static List<Collection> collections;

        public AllBooksFromFile(ICollectionsRepository collectRep)
        {
            this.collectionsRepository = collectRep;
            collections = collectRep.GetOne(MainId);
            string jsonString = File.ReadAllText("Data/" + MainId + "/books.json");

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
            System.IO.File.Delete("Data/" + MainId + "/Texts/" + book.Name + ".txt");

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
