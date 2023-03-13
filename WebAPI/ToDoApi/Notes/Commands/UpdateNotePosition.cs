using MediatR;
using SendGrid.Helpers.Errors.Model;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.Notes.Commands
{
    public class UpdateNotePosition
    {
        public class Command : IRequest<bool>
        {
            public Guid ListId { get; set; }
            public Guid NoteId { get; set; }
            public int Position { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<Note> _noteRepository;
            private readonly IRepository<ToDoList> _listRepository;

            public RequestHandler(IRepository<Note> noteRepository, IRepository<ToDoList> listRepository)
            {
                _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
                _listRepository = listRepository ?? throw new ArgumentNullException(nameof(listRepository));
            }

            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var list = _listRepository.GetById(request.ListId);
                if (list == null)
                {
                    throw new NotFoundException();
                }

                var note = list.Notes.FirstOrDefault(x => x.Id == request.NoteId);
                if (note == null)
                {
                    throw new NotFoundException();
                }

                var lists = _noteRepository.Find(x => x.Position == request.Position);
                if (lists.FirstOrDefault() == null)
                {
                    throw new BadRequestException();
                }

                if (request.Position > note.Position)
                {
                    var toDoLists = _noteRepository.Find(x => x.Position > note.Position && x.ToDoListId == request.ListId && x.ToDoListId == request.ListId);

                    foreach (var item in toDoLists)
                    {
                        item.Position--;
                    }

                    note.Position = request.Position;
                    _noteRepository.SaveChanges();
                }
                else if (request.Position < note.Position)
                {
                    var toDoLists = _noteRepository.Find(x => x.Position < note.Position && x.Position >= request.Position && x.ToDoListId == request.ListId);

                    foreach (var item in toDoLists)
                    {
                        item.Position++;
                    }

                    note.Position = request.Position;
                    _noteRepository.SaveChanges();
                }
                return Task.FromResult(true);

            }
        }
    }
}
