using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotesApp.Models
{
    public class AppUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Note>? Notes { get; set; }
        
        public int? GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender? Gender { get; set; }
        public string? ImageFile { get; set; }
        public ICollection<HoppyUser>? HoppyUser { get; set; }
    }
}
