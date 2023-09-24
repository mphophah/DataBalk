using AutoMapper;

namespace UserTaskApi.Controllers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, LoginDto>();
            CreateMap<User, RegisterDto>(); 
            CreateMap<Task, TaskDto>();
            CreateMap<User, UpdateUserDto>();
        }
    }
}