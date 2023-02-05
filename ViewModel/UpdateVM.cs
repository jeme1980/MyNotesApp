using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyNotesApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyNotesApp.ViewModel
{
    public class UpdateVM
    {
        public string Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public int? GenderId { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? GenderList { get; set; }
        public string? ImageFile { get; set; }
        public IFormFile? file { get; set; }
        public List<int>? HoppyId { get; set; }
    }
}
