using Microsoft.EntityFrameworkCore;
using MyNotesApp.Core.IRepository;
using MyNotesApp.Data;
using System.Linq.Expressions;

namespace MyNotesApp.Core.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> entity;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entity = context.Set<T>();
        }
        public void Cteate(T t)
        {
            entity.Add(t);
        }

        public void Delete(int? id)
        {
            var currententity = entity.Find(id);
            entity.Remove(currententity);
        }


        public List<T> GetAll()
        {
            return entity.ToList();
        }

        public List<T> GetAllBy(Expression<Func<T, bool>> predicate)
        {
            return entity.Where(predicate).ToList();
        }

        public T GetBy(Expression<Func<T, bool>> predicate)
        {
            return entity.FirstOrDefault(predicate);
        }

        public void Update(T t)
        {
            entity.Update(t);
        }
    }
}
