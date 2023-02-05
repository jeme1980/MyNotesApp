using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyNotesApp.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotesApp.Models
{
    public class Student
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }
        [Required]
        [Display(Name ="student name")]
        public string? StudentFirstName { get; set;}
        [Required]
        public string? StudentLasttName { get; set; }
        [NotMapped]
        [ValidateNever]
        public List<SelectListItem>? SH { get; set; }
        public ICollection<StudentHoppy> studentHoppies { get; set; } = new HashSet<StudentHoppy>();
    }
}
