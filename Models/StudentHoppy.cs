using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNotesApp.Models
{
    public class StudentHoppy
    {
        [ForeignKey("Hoppy")]
        public int HoppyId { get; set; }
        [ForeignKey("Student")]
        public int? StudentId { get; set; }
        public Hoppy? Hoppy { get; set; }
        public Student? Student { get; set; }
    }
}
