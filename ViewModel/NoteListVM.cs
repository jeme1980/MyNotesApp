using System.ComponentModel.DataAnnotations;

namespace MyNotesApp.ViewModel
{
    public class NoteListVM
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public bool IsSelected { get; set; }
    }
}
