using MediatR;
using ToDoApi.Notes.Models;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Commands
{
    public static class UpdateNote
    {
        public class Command : IRequest<bool>
        {
            public Guid ToDoListId { get; set; }
            public Guid NotetId { get; set; }
            public NoteModel Model { get; set; } = null!;
        }

        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<ToDoList> _repository;
            private readonly IRepository<Note> _noteRepository;

            public RequestHandler(IRepository<ToDoList> repository, IRepository<Note> noteRepository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
            }

            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var list = _repository.GetById(request.ToDoListId);
                if (list == null)
                {
                    return Task.FromResult(false);
                }

                var note = list.Notes.FirstOrDefault(x => x.Id == request.NotetId);
                if (note == null)
                    return Task.FromResult(false);

                note.Content = request.Model.Content;
                note.IsComplete = request.Model.IsCompleted;

                _repository.SaveChanges();
                return Task.FromResult(true);
            }
        }
    }
}
