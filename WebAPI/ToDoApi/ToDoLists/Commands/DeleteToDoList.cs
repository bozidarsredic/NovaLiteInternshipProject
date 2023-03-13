using MediatR;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Commands
{
    public static class DeleteToDoList
    {
        public class Command : IRequest<bool>
        {
            public Guid Id { get; set; }
            public string Owner { get; set; } = string.Empty;
        }

        public class RequestHandler : IRequestHandler<Command, bool>
        {
            private readonly IRepository<ToDoList> _repository;

            public RequestHandler(IRepository<ToDoList> repository)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var toDoList = _repository.GetById(request.Id);
                if (toDoList == null)
                {
                    return Task.FromResult(false);
                }
                if (toDoList.Owner != request.Owner)
                {
                    return Task.FromResult(false);
                }
                _repository.Delete(toDoList);
                _repository.SaveChanges();

                var toDoLists = _repository.Find(x => x.Position > toDoList.Position);

                foreach (var item in toDoLists)
                {
                    item.Position--;
                }
                _repository.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}
