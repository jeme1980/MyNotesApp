using MyNotesApp.Models;

namespace MyNotesApp.ViewModel
{
    public class NewUserDropDownsVM
    {
        public NewUserDropDownsVM()
        {
            Genders = new List<Gender>();
            Hoppies = new List<Hoppy>();
        }
        public List<Gender> Genders { get; set; }
        public List<Hoppy> Hoppies { get; set; }
    }
}
