using AutoMapper;
using dotnet_RPG.Dtos.Character;
using dotnet_RPG.Dtos.Character.User;
using dotnet_RPG.Models;

namespace dotnet_RPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            
        }
    }
}