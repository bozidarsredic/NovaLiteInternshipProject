using MediatR;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Commands
{
    public static class DeleteNote
    {
        public class Command : IRequest<bool>
        {
            public Guid ToDoListId { get; set; }
            public Guid NoteId { get; set; }
        }
        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<ToDoList> _toDoListRepository;
            private readonly IRepository<Note> _noteRepository;

            public RequestHandler(IRepository<ToDoList> toDoListRepository, IRepository<Note> noteRepository)
            {
                _toDoListRepository = toDoListRepository ?? throw new ArgumentNullException(nameof(toDoListRepository));
                _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
            }

            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var list = _toDoListRepository.GetById(request.ToDoListId);
                if (list == null)
                {
                    return Task.FromResult(false);
                }

                var note = _noteRepository.GetById(request.NoteId);
                if (note == null)
                {
                    return Task.FromResult(false);
                }

                _noteRepository.Delete(note);
                _noteRepository.SaveChanges();

                var notes = _noteRepository.Find(x => x.Position > note.Position && x.ToDoListId == request.ToDoListId);

                foreach (var item in notes)
                {
                    item.Position--;
                }
                _noteRepository.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}
