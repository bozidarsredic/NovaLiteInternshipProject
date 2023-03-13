using Microsoft.EntityFrameworkCore;
using ToDo.Infrastructure;
using ToDoCore;

namespace ToDoApi.Repositories
{
    public class ToDoListRepository : GenericRepository<ToDoList>
    {
        public ToDoListRepository(ToDoDbContext context) : base(context)
        {
        }
        public override ToDoList GetById(Guid id)
        {
            return _context.ToDoLists.Include(list => list.Notes).FirstOrDefault(x => x.Id == id)!;
        }

        public override IEnumerable<ToDoList> GetAll()
        {
            return _context.ToDoLists.Include(x => x.Notes.OrderBy(note => note.Position)).OrderByDescending(x => x.Position);
        }
    }
}
