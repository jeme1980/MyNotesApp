using MyNotesApp.Core.IRepository;
using MyNotesApp.Data;
using MyNotesApp.Models;

namespace MyNotesApp.Core.Repository
{
    public class NoteRepository : Repository<Note>, INotesRepository
    {
        public NoteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
