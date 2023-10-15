using WeddingPlanner.Data;
using WeddingPlanner.Models;
using WeddingPlanner.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WeddingPlanner.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginContext _context;

        public LoginController(LoginContext loginContext)
        {
            _context = loginContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login_in([Bind(Prefix = "Login")] UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                User? userInDb = _context.Users
                    .Select(
                        a =>
                            new User
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Email = a.Email,
                                Password = a.Password
                            }
                    )
                    .SingleOrDefault(u => u.Email == userDTO.Email);
                if (userInDb == null)
                {
                    return View("Index");
                }

                PasswordHasher<UserDTO> hasher = new PasswordHasher<UserDTO>();

                var result = hasher.VerifyHashedPassword(
                    userDTO,
                    userInDb.Password,
                    userDTO.Password
                );

                if (result == 0)
                {
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.Id);
                    HttpContext.Session.SetString("UserName", userInDb.Name);
                    return RedirectToAction("Index","Wedding");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost("create")]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();

                user.Password = Hasher.HashPassword(user, user.Password);

                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index");
            }
        }
    }
}
