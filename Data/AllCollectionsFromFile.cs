using Microsoft.Extensions.Options;
using System.Text.Json;
using Documents.Data;
using Documents.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Documents.Data
{
    public class AllCollectionsFromFile : ICollectionsRepository
    {
        public Guid MainId = Guid.Parse(File.ReadAllText("Data/MainId.txt"));
        private static List<UserData> collections = new List<UserData>();

        public AllCollectionsFromFile()
        {
            string jsonString = File.ReadAllText("Data/collections.json");
            collections = JsonSerializer.Deserialize<List<UserData>>(jsonString);
        }

        public List<Collection> GetOne(Guid id)
        {
            string jsonString = File.ReadAllText("Data/collections.json");
            collections = JsonSerializer.Deserialize<List<UserData>>(jsonString);

            List<Collection> list = new List<Collection>();
            foreach (var collection in collections)
            {
                if(collection.DataId == id)
                {
                    list = collection.Collections;
                    break;
                }
            }
            
            return list;
        }

        public void ResetCollection()
        {
            string jsonString = File.ReadAllText("Data/books.json");
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(jsonString);

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
            string newcol = JsonSerializer.Serialize(collections, options);
            File.WriteAllText("Data/collections.json", newcol);
        }

        public void ResaveUserData(List<Collection> collections)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            string stringUD = File.ReadAllText("Data/collections.json");
            List<UserData> Userdatas = JsonSerializer.Deserialize<List<UserData>>(stringUD);
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
