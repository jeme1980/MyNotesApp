using MyNotesApp.Core.IRepository;
using MyNotesApp.Data;
using MyNotesApp.Models;

namespace MyNotesApp.Core.Repository
{
    public class HoppyRepository : Repository<Hoppy>, IHoppiesRepository
    {
        public HoppyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
