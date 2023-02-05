using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotesApp.Models
{
    public class HoppyUser
    {
        [ForeignKey("Hoppy")]
        public int HoppyId { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public Hoppy? Hoppy { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
