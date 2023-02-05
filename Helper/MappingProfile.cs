using AutoMapper;
using MyNotesApp.Models;
using MyNotesApp.ViewModel;

namespace MyNotesApp.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Note, NoteVM>().ReverseMap();
            CreateMap<Hoppy, HoppyVM>().ReverseMap();
        }
    }
}
