using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WriterApp.Models;
using System.IO;
using static System.Reflection.Metadata.BlobBuilder;
using WriterApp.Data;

namespace WriterApp.Controllers
{
    public class UserController : Controller
    {
        IUserRepository userRepository;
        List<User> users;

        public UserController(IUserRepository userRep)
        {
            userRepository = userRep;
            users = userRepository.GetAll();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registration(string Name, string Email)
        {
            bool flag = true;

            foreach (var u in users)
            {
                if (u.Email == Email)
                {
                    flag = false;
                    System.IO.File.WriteAllText("Data/MainId.txt", Convert.ToString(u.UserId));
                    break;
                }
            }
            if (flag)
            {
                User user = new User(Name, Email);
                users.Add(user);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string newuser = JsonSerializer.Serialize(users, options);
                System.IO.File.WriteAllText("Data/users.json", newuser);

                userRepository.Save(user.UserId);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
