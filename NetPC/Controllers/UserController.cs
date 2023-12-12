using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetPC.Models;

namespace NetPC.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public IActionResult AddUser([FromForm] User model)
        {
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                TempData["Alert"] = "Nazwa użytkownika jest zajęta";
                return RedirectToAction("Register");
            }
            else
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt();

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash, salt);

                _context.Users.Add(new User
                {
                    Username = model.Username,
                    PasswordHash = hashedPassword,
                    Salt = salt
                });

                _context.SaveChanges();

                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromForm] User model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.PasswordHash, user.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                var principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal);

                TempData["Alert"] = null;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Alert"] = "Błędne dane logowania";
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> LogoutUser()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
