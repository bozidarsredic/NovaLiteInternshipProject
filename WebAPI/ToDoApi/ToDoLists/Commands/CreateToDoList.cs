using MediatR;
using ToDoApi.Repositories;
using ToDoCore;
namespace ToDoApi.ToDoLists.Commands
{
    public static class CreateToDoList
    {
        public class Command : IRequest<ToDoList?>
        {
            public string Title { get; set; } = string.Empty;
            public string Owner { get; set; } = string.Empty;
            public DateTime? Redminder { get; set; }
            public int Position { get; set; }
        }

        public class RequestHandler : IRequestHandler<Command, ToDoList?>
        {
            private readonly IRepository<ToDoList> _repository;

            public RequestHandler(IRepository<ToDoList> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public Task<ToDoList?> Handle(Command request, CancellationToken cancellationToken)
            {
                var toDoLists = _repository.Find(x => x.Owner == request.Owner);

                var toDoList = new ToDoList
                {
                    Owner = request.Owner,
                    Remainder = request.Redminder,
                    Title = request.Title,
                    Position = toDoLists.Count()

                };

                _repository.Create(toDoList);
                _repository.SaveChanges();

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return Task.FromResult(toDoList);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.


            }
        }
    }
}
