using System.Linq;
using AutoMapper;
using dotnet_RPG.Dtos.Character;
using dotnet_RPG.Dtos.Character.Skill;
using dotnet_RPG.Dtos.Character.User;
using dotnet_RPG.Dtos.Character.Weapon;
using dotnet_RPG.Models;

namespace dotnet_RPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>()
                .ForMember(dto => dto.skills, c=>c.MapFrom(c => c.CharacterSkills.Select(cs =>cs.skill))); //this allow us jump straign to skill without going through the joining entity characterskill
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<AddWeaponDto, Weapon>();

            CreateMap<Skill, GetSkillDto>();
        }
    }
}