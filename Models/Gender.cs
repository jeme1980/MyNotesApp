using Microsoft.Build.Framework;

namespace MyNotesApp.Models
{
    public class Gender
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public List<AppUser>? AppUsers { get; set; }
    }
}
