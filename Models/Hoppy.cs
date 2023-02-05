namespace MyNotesApp.Models
{
    public class Hoppy
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<HoppyUser>? HoppyUser { get; set; }
        public ICollection<StudentHoppy> studentHoppies { get; set; }
    }
}
