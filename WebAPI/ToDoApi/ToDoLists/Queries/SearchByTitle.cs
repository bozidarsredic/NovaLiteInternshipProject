using MediatR;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Queries
{
    public class SearchByTitle
    {
        public class Query : IRequest<IEnumerable<ToDoList>?>
        {
            public string Title { get; set; } = String.Empty;
            public string Owner { get; set; } = string.Empty;
        }

        public class RequestHandler : IRequestHandler<Query, IEnumerable<ToDoList>?>
        {
            private readonly IRepository<ToDoList> _repository;

            public RequestHandler(IRepository<ToDoList> repository)
            {
                _repository = repository;
            }

            public Task<IEnumerable<ToDoList>?> Handle(Query request, CancellationToken cancellationToken)
            {
                var toDoLists = _repository.Find(x => x.Title.Contains(request.Title) && x.Owner == request.Owner);
                if (toDoLists == null)
                {
                    return Task.FromResult(null as IEnumerable<ToDoList>);
                }

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return Task.FromResult(toDoLists);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            }
        }
    }

}
