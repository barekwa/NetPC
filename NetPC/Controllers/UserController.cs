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
            // Check if the username is already taken
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                TempData["Alert"] = "Username is already taken.";
                return RedirectToAction("Register");
            }
            else
            {
                // Generate salt and hash the password using BCrypt
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

        // Handles the login of a user
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromForm] User model)
        {
            // Find the user with the provided username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.PasswordHash, user.PasswordHash))
            {
                // If the password is correct, create claims for authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };

                // Create an identity and principal for the authenticated user
                var userIdentity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal);

                TempData["Alert"] = null;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Alert"] = "Incorrect login credentials.";
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
