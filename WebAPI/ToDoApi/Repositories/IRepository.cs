using System.Linq.Expressions;

namespace ToDoApi.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        void Delete(T entity);
        void Create(T entity);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void SaveChanges();
    }
}
