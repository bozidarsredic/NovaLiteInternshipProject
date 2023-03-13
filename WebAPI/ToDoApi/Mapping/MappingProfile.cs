using AutoMapper;
using ToDoApi.Notes.Models;
using ToDoCore;

namespace ToDoApi.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            _ = CreateMap<Note, GetNotesModel>().ReverseMap();

            _ = CreateMap<ToDoList, Response>();
        }
    }
}
