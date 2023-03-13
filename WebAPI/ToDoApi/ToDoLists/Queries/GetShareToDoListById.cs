using AutoMapper;
using MediatR;
using ToDoApi.Notes.Models;
using ToDoApi.Repositories;
using ToDoCore;

namespace ToDoApi.ToDoLists.Queries
{
    public static class GetShareToDoListById
    {



        public class Query : IRequest<Response?>
        {
            public Guid Id { get; set; }
            // public string Owner { get; set; } = String.Empty;
        }


        public class RequestHandler : IRequestHandler<Query, Response?>
        {
            private readonly IRepository<ToDoList> _repository;
            private readonly IMapper _mapper;

            public RequestHandler(IRepository<ToDoList> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public Task<Response?> Handle(Query request, CancellationToken cancellationToken)
            {
                var toDoList = _repository.GetById(request.Id);
                if (toDoList == null || toDoList.ShareTime == null)
                {
                    return Task.FromResult(null as Response);
                }
                var a = toDoList.ShareTime;
                var b = DateTime.Now.AddHours(-3);

                if (toDoList.ShareTime < DateTime.Now.AddHours(-3))
                {
                    return Task.FromResult(null as Response);
                }

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return Task.FromResult(_mapper.Map<Response>(toDoList));
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

            }




        }


    }
}

