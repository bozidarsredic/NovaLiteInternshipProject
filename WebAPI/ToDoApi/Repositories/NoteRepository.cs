using ToDo.Infrastructure;
using ToDoCore;

namespace ToDoApi.Repositories
{
    public class NoteRepository : GenericRepository<Note>
    {
        public NoteRepository(ToDoDbContext context) : base(context)
        {
        }
    }
}
