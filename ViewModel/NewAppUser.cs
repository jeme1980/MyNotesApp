using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MyNotesApp.ViewModel
{
    public class NewAppUser
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public int? GenderId { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        public string? ImageFile { get; set; }
        [ValidateNever]
        public IFormFile? file { get; set; }
        public string? roleName { get; set; }
        public List<HoppyChBoxVM> HoppyChBoxVMs { get; set; } = new List<HoppyChBoxVM>();
    }
}
