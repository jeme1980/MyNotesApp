using MyNotesApp.Core.IRepository;
using MyNotesApp.Data;
using MyNotesApp.Models;

namespace MyNotesApp.Core.Repository
{
    public class GenderRepository : Repository<Gender>, IGendersRepository
    {
        public GenderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
