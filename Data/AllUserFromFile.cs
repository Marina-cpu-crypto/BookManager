using Microsoft.Extensions.Options;
using System.Text.Json;
using Documents.Data;
using Documents.Models;
using Documents.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Documents.Data
{
    public class AllUsersFromFile : IUserRepository
    {
        public AllUsersFromFile() { }
        public List<User> GetAll()
        {
            string jsonString = System.IO.File.ReadAllText("Data/users.json");
            var users = JsonSerializer.Deserialize<List<User>>(jsonString) ?? new List<User>();

            return users;
        }
        public void Save(Guid UserId)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };


            UserData userData = new UserData(UserId);
            string stringUD = System.IO.File.ReadAllText("Data/collections.json");
            List<UserData> Userdatas = JsonSerializer.Deserialize<List<UserData>>(stringUD) ?? new List<UserData>();
            Userdatas.Add(userData);
            string newusersdata = JsonSerializer.Serialize(Userdatas, options);
            System.IO.File.WriteAllText("Data/collections.json", newusersdata);

            System.IO.File.WriteAllText("Data/MainId.txt", Convert.ToString(UserId));
            Directory.CreateDirectory("Data/" + Convert.ToString(UserId));
        }
    }
}
