using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Documents.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Documents.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registration(string Name, string Email)
        {
            string jsonString = System.IO.File.ReadAllText("Data/users.json");
            var users = JsonSerializer.Deserialize<List<User>>(jsonString);

            User user = new User(Name, Email);

            if (users.Count == 1)
            {
                users.Add(user);
                UserData userData = new UserData(user.UserId);

                var options = new JsonSerializerOptions { WriteIndented = true };

                string newuser = JsonSerializer.Serialize(users, options);
                System.IO.File.WriteAllText("Data/users.json", newuser);

                string stringUD = System.IO.File.ReadAllText("Data/collections.json");
                List<UserData> Userdatas = JsonSerializer.Deserialize<List<UserData>>(stringUD);
                Userdatas.Add(userData);

                string newusersdata = JsonSerializer.Serialize(Userdatas, options);
                System.IO.File.WriteAllText("Data/collections.json",newusersdata);
            }
            else
            {
                foreach (var u in users)
                {
                    if (u.Email == Email)
                    {
                        System.IO.File.WriteAllText("Data/MainId.txt", Convert.ToString(u.UserId));
                        break;
                    }
                    
                }
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
