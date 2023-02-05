using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MyNotesApp.Models;

namespace MyNotesApp.ViewModel
{
    public class CreateStudentVM
    {
        public int StudentId { get; set; }
        [Required]
        [Display(Name = "student name")]
        public string? StudentFirstName { get; set; }
        [Required]
        public string? StudentLasttName { get; set; }
        public List<HoppyChBoxVM>? HoppyChBoxVMs { get; set; }
    }
}
