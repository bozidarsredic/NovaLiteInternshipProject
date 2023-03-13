using AutoMapper;
using MediatR;
using ToDoApi.Notes.Models;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Queries
{
    public static class GetNotesByListId
    {
        public class Query : IRequest<IEnumerable<GetNotesModel>?>
        {
            public Guid Id { get; set; }
        }

        public class RequestHandler : IRequestHandler<Query, IEnumerable<GetNotesModel>?>
        {
            private readonly IRepository<ToDoList> _repository;

            private readonly IMapper _mapper;
            public RequestHandler(IRepository<ToDoList> repository, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _mapper = mapper;

            }

            public Task<IEnumerable<GetNotesModel>?> Handle(Query request, CancellationToken cancellationToken)
            {
                var list = _repository.GetById(request.Id);

                if (list == null)
                {
                    return Task.FromResult(null as IEnumerable<GetNotesModel>);
                }
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return Task.FromResult(_mapper.Map<List<GetNotesModel>>(list.Notes).AsEnumerable());
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            }



        }
    }
}
