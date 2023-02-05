using Microsoft.AspNetCore.Mvc.Rendering;
using MyNotesApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotesApp.ViewModel
{
    public class NoteVM
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
        [Required]
        public string? Color { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
    }
}
