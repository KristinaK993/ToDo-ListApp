using AutoMapper;
using ToDoApp.API.Models.DTOs;
using ToDoApp.API.Models.Entities;

namespace ToDoApp.API.Mappings
{
    public class AutoMapperProfile : Profile //ärver från automappers profile klass
    {
        public AutoMapperProfile() //konstuktor för att konfigurera alla mappings
        {
            // User
            CreateMap<User, UserDTO>(); //returnera användare från API
            CreateMap<UserRegisterDto, User>(); //när någon registrerar sig 
            CreateMap<UserUpdateDTO, User>(); //när någon uppdaterar sin profil
        }
    }
}
