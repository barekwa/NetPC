using System.ComponentModel.DataAnnotations;

namespace NetPC.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int Phone { get; set; }

        public DateTime BirthDate { get; set; }

        public string Category { get; set; }

        public string? Subcategory { get; set;}

    }
}