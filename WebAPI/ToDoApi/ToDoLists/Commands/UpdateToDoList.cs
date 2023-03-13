using MediatR;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Commands
{
    public static class UpdateToDoList
    {
        public class Command : IRequest<bool>
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public DateTime? Remainder { get; set; }
            public string Owner { get; set; } = string.Empty;
            public DateTime? ShareTime { get; set; }
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
                var list = _repository.GetById(request.Id);
                if (list == null)
                {
                    return Task.FromResult(false);
                }
                if (list.Owner != request.Owner)
                {
                    return Task.FromResult(false);
                }

                list.Remainder = request.Remainder;
                list.Title = request.Title;
                list.ShareTime = request.ShareTime;

                _repository.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}
