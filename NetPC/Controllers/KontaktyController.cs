using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetPC.Models;

namespace NetPC.Controllers
{
    public class KontaktyController : Controller
    {

        private readonly ApplicationDbContext _context;

        public KontaktyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "RequireLoggedIn")]
        public IActionResult AddContact()
        {
            var categories = _context.Categories.Select(c => c.Name).ToList();
            var subcategories = _context.Subcategories.Select(s => s.Name).ToList();

            ViewBag.Categories = new SelectList(categories);
            ViewBag.Subcategories = new SelectList(subcategories);

            return View("AddContact");
        }

        [Authorize(Policy = "RequireLoggedIn")]
        public IActionResult UpdateContact(int id)
        {
            var contact = _context.Contacts.FirstOrDefault(k => k.Id == id);
            var categories = _context.Categories.Select(c => c.Name).ToList();
            var subcategories = _context.Subcategories.Select(s => s.Name).ToList();

            ViewBag.Categories = new SelectList(categories, contact.Category);
            ViewBag.Subcategories = new SelectList(subcategories, contact.Subcategory);

            return View("UpdateContact", contact);
        }

        
        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        public IActionResult Create([FromForm] Contact contact)
        {
            if(contact == null)
            {
                return BadRequest();
            }

            var existingContact = _context.Contacts.FirstOrDefault(c => c.Email == contact.Email);

            if(existingContact == null)
            {
                _context.Add(contact);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["EmailExistsMessage"] = "Podany adres e-mail już istnieje.";
                return RedirectToAction("AddContact");
            }       
        }

        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        public IActionResult Update([FromForm] Contact updatedContact, int id)
        {
            var contactToUpdate = _context.Contacts.FirstOrDefault(k => k.Id == id);

            if (contactToUpdate == null)
            {
                return NotFound();
            }

            var existingContact = _context.Contacts.FirstOrDefault(c => c.Email == updatedContact.Email && c.Id != id);

            if (existingContact != null)
            {
                TempData["EmailExistsMessage"] = "Podany adres e-mail już istnieje.";
                return View("UpdateContact", contactToUpdate);
            }

            contactToUpdate.Name = updatedContact.Name;
            contactToUpdate.Surname = updatedContact.Surname;
            contactToUpdate.Email = updatedContact.Email;
            contactToUpdate.Password = updatedContact.Password;
            contactToUpdate.Category = updatedContact.Category;
            contactToUpdate.Subcategory = updatedContact.Subcategory;
            contactToUpdate.Phone = updatedContact.Phone;
            contactToUpdate.BirthDate = updatedContact.BirthDate;

            if (updatedContact.Category == "prywatny")
            {
                contactToUpdate.Subcategory = null;
            }

            _context.Update(contactToUpdate);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        public IActionResult Delete(int id)
        {
            var contactToDelete = _context.Contacts.FirstOrDefault(k => k.Id == id);
            if (contactToDelete == null)
            {
                return NotFound();
            }
            _context.Contacts.Remove(contactToDelete);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult Details(int id)
        {
			var contact = _context.Contacts.FirstOrDefault(k => k.Id == id);
			return View("Details", contact);
        }
    }
}
