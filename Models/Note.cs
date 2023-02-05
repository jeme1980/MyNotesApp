using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotesApp.Models
{
    public class Note
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName ="nvarchar(100)")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? AppUser { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
