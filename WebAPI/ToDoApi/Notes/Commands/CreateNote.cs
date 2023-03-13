using AutoMapper;
using MediatR;
using ToDoApi.Notes.Models;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Commands
{
    public static class CreateNote
    {
        public class Command : IRequest<GetNotesModel?>
        {
            public Guid Id { get; set; }
            public NoteModel Model { get; set; } = null!;
        }

        public class RequestHandler : IRequestHandler<Command, GetNotesModel?>
        {
            private readonly IRepository<ToDoList> _repository;
            private readonly IMapper _mapper;
            public RequestHandler(IRepository<ToDoList> repository, IRepository<Note> noteRepository, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _mapper = mapper;
            }

            public Task<GetNotesModel?> Handle(Command request, CancellationToken cancellationToken)
            {
                var list = _repository.GetById(request.Id);
                if (list == null)
                {
                    return Task.FromResult(null as GetNotesModel);
                }

                var note = new Note
                {
                    ToDoListId = list.Id,
                    ToDoList = list,
                    Content = request.Model.Content,
                    IsComplete = request.Model.IsCompleted,
                    Position = list.Notes.Count()
                };

                list.Notes.Add(note);
                _repository.SaveChanges();

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return Task.FromResult(_mapper.Map<GetNotesModel>(note));
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            }
        }
    }
}
