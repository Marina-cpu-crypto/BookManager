using Documents.Data;
using System.Text.Json;
using Documents.Models;

namespace Documents.Data
{
    public class GetAllCollections : ICollectionsRepository
    {
        private static List<Collection> collections = new List<Collection>();

        public GetAllCollections()
        {
            string jsonString = File.ReadAllText("Data/collections.json");
            collections = JsonSerializer.Deserialize<List<Collection>>(jsonString);
        }

        public List<Collection> GetAll()
        {
            string jsonString = File.ReadAllText("Data/collections.json");
            collections = JsonSerializer.Deserialize<List<Collection>>(jsonString);

            return collections;
        }

        //public string? TryGetById(Guid id)
        //{
        //    return collections.FirstOrDefault(c => c.CollectionId == id);
        //}
    }
}