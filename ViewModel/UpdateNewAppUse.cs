using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MyNotesApp.ViewModel
{
    public class UpdateNewAppUser
    {
        public string Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public int? GenderId { get; set; }
        public string? ImageFile { get; set; }
        [ValidateNever]
        public IFormFile? file { get; set; }
        public List<HoppyChBoxVM>? HoppyChBoxVMs { get; set; } = new List<HoppyChBoxVM>();
    }
}
