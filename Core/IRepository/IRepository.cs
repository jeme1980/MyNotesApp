using System.Linq.Expressions;
namespace MyNotesApp.Core.IRepository
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        List<T> GetAllBy(Expression<Func<T,bool>> predicate);
        T GetBy(Expression<Func<T,bool>> predicate);
        void Cteate(T t);
        void Delete(int? id);
        void Update(T t);
    }
}
