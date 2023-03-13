using AutoMapper;
using MediatR;
using ToDoApi.Notes.Models;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Queries
{
    public static class GetToDoLists
    {
        public class Query : IRequest<IEnumerable<Response>>
        {
            public string Owner { get; set; } = String.Empty;
        }
        public class RequestHandler : IRequestHandler<Query, IEnumerable<Response>>
        {
            private readonly IRepository<ToDoList> _repository;
            private readonly IRepository<Note> _noteRepository;
            private readonly IMapper _mapper;

            public RequestHandler(IRepository<ToDoList> repository, IRepository<Note> noteRepository, IMapper mapper)
            {
                _repository = repository;
                _noteRepository = noteRepository;
                _mapper = mapper;
            }

            public Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var toDoLists = _repository.Find(x => x.Owner == request.Owner).OrderByDescending(x => x.Position);
                var response = _mapper.Map<IEnumerable<Response>>(toDoLists);
                return Task.FromResult(response);
            }
        }

    }
}
