using System.Text.Json;
using Documents.Models;

namespace Documents.Data
{
    public class GetAllCollections : ICollectionsRepository
    {
        private const string CollectionsFilePath = "Data/collections.json";

        public List<Collection> GetAll()
        {
            try
            {
                if (!File.Exists(CollectionsFilePath))
                    return new List<Collection>();

                string jsonString = File.ReadAllText(CollectionsFilePath);
                
                if (string.IsNullOrWhiteSpace(jsonString))
                    return new List<Collection>();

                var collections = JsonSerializer.Deserialize<List<Collection>>(jsonString);
                return collections ?? new List<Collection>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке коллекций: {ex.Message}");
                return new List<Collection>();
            }
        }
    }
}
