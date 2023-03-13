using MediatR;
using SendGrid.Helpers.Errors.Model;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Commands
{
    public class UpdateToDoListPosition
    {
        public class Command : IRequest<bool>
        {
            public Guid Id { get; set; }
            public int Position { get; set; }
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
                if (toDoList.Owner != request.Owner)
                {
                    throw new NotFoundException();
                }

                if (toDoList == null)
                {
                    throw new NotFoundException();
                }
                var lists = _repository.Find(x => x.Position == request.Position && x.Owner == request.Owner);
                if (lists.FirstOrDefault() == null)
                {
                    throw new BadRequestException();
                }


                if (request.Position > toDoList.Position)
                {
                    var toDoLists = _repository.Find(x => x.Position > toDoList.Position && x.Position <= request.Position && x.Owner == request.Owner);

                    foreach (var item in toDoLists)
                    {
                        item.Position--;
                    }

                    toDoList.Position = request.Position;
                    _repository.SaveChanges();
                }
                else if (request.Position < toDoList.Position)
                {
                    var toDoLists = _repository.Find(x => x.Position < toDoList.Position && x.Position >= request.Position && x.Owner == request.Owner);

                    foreach (var item in toDoLists)
                    {
                        item.Position++;
                    }

                    toDoList.Position = request.Position;
                    _repository.SaveChanges();
                }
                return Task.FromResult(true);

            }
        }
    }
}
