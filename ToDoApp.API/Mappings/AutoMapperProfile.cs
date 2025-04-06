using AutoMapper;
using ToDoApp.API.Models.DTOs;
using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User
            CreateMap<User, UserDTO>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateDTO, User>();
        }
    }
}
