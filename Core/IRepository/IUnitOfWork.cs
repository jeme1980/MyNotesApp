using MyNotesApp.Core.Repository;
using MyNotesApp.Models;

namespace MyNotesApp.Core.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        INotesRepository Notes { get; }
        IHoppiesRepository Hoppies { get; }
        IGendersRepository Genders { get; }
        void Save();
    }
}
