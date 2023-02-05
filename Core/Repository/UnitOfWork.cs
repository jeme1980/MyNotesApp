using Microsoft.AspNetCore.Identity;
using MyNotesApp.Core.IRepository;
using MyNotesApp.Data;
using MyNotesApp.Models;

namespace MyNotesApp.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Notes = new NoteRepository(_context);
            Hoppies = new HoppyRepository(_context);
            Genders = new GenderRepository(_context);

        }
        public INotesRepository Notes { get; private set; }
        public IHoppiesRepository Hoppies { get; private set; }
        public IGendersRepository Genders { get; private set; }
        public void Dispose()
        {
            _context.Dispose(); 
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
