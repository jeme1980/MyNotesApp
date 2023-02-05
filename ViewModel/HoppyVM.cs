using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyNotesApp.ViewModel
{
    public class HoppyVM
    {
        public int Id { get; set; }
        [DisplayName("תחביבים")]
        public string? Name { get; set; }
    }
}
