using Documents.Data;
using Documents.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Documents.Data;
using Documents.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Documents.Data
{
    public class AllCollectionsFromFile : ICollectionsRepository
    {
        private static List<UserData> collections = new List<UserData>();

        public AllCollectionsFromFile()
        {
            string jsonString = File.ReadAllText("Data/collections.json");
            collections = JsonSerializer.Deserialize<List<UserData>>(jsonString) ?? new List<UserData>();
        }
        public List<UserData> GetAll()
        {
            return collections;
        }
        public List<Collection> GetOne(Guid id)
        {
            List<Collection> list = new List<Collection>();
            foreach (var collection in collections)
            {
                if (collection.DataId == id)
                {
                    list = collection.Collections;
                    break;
                }
            }

            return list;
        }
        public Book TryGetById(Guid bookid)
        {
            Guid MainId = Guid.Parse(System.IO.File.ReadAllText("Data/MainId.txt"));

            List<Collection> list = this.GetOne(MainId);
            Book book = null;

            foreach (var c in list)
            {
                foreach (var b in c.Books)
                {
                    if (b.Id == bookid) book = b;
                }
            }

            return book;
        }
        public void ResaveUserData()
        {
            Guid MainId = Guid.Parse(System.IO.File.ReadAllText("Data/MainId.txt"));
            List<Collection> list = this.GetOne(MainId);
            var options = new JsonSerializerOptions { WriteIndented = true };

            string stringUD = File.ReadAllText("Data/collections.json");
            List<UserData> Userdatas = JsonSerializer.Deserialize<List<UserData>>(stringUD) ?? new List<UserData>();
            for (int i = 0; i < Userdatas.Count; i++)
            {
                if (Userdatas[i].DataId == MainId)
                {
                    Userdatas[i].Collections = list;
                    break;
                }
            }

            string newusersdata = JsonSerializer.Serialize(Userdatas, options);
            File.WriteAllText("Data/collections.json", newusersdata);
        }

        public void ResetCollection()
        {
            Guid MainId = Guid.Parse(File.ReadAllText("Data/MainId.txt"));

            string jsonString = File.ReadAllText("Data/" + MainId + "/books.json");
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(jsonString) ?? new List<Book>();

            var thiscoll = this.GetOne(MainId);

            foreach (var book in books)
            {
                int ind = 0;
                bool flag = false;
                if (book.IsDone) ind = 1;

                foreach (var b in thiscoll[ind].Books) if (b.Id == book.Id) flag = true;

                if (!flag) thiscoll[ind].Books.Add(book);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };

            ResaveUserData(thiscoll);
        }

        public void ResaveUserData(List<Collection> collections)
        {
            Guid MainId = Guid.Parse(File.ReadAllText("Data/MainId.txt"));

            var options = new JsonSerializerOptions { WriteIndented = true };

            string stringUD = File.ReadAllText("Data/collections.json");
            List<UserData> Userdatas = JsonSerializer.Deserialize<List<UserData>>(stringUD) ?? new List<UserData>();
            for (int i = 0; i < Userdatas.Count; i++)
            {
                if (Userdatas[i].DataId == MainId)
                {
                    Userdatas[i].Collections = collections;
                    break;
                }
            }

            string newusersdata = JsonSerializer.Serialize(Userdatas, options);
            File.WriteAllText("Data/collections.json", newusersdata);
        }
    }
}
