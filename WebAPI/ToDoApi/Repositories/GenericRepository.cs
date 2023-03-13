using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDo.Infrastructure;

namespace ToDoApi.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly ToDoDbContext _context;
        private DbSet<T> dbSet;

        protected GenericRepository(ToDoDbContext context)
        {
            _context = context;
            this.dbSet = context.Set<T>();
        }
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate).AsEnumerable();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }
        public virtual T GetById(Guid id)
        {
            return dbSet.Find(id)!;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
