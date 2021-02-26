using System.Collections.Generic;
using dotnet_RPG.Dtos.Character.Skill;
using dotnet_RPG.Dtos.Character.Weapon;
using dotnet_RPG.Models;

namespace dotnet_RPG.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class {get; set;} = RpgClass.Knigth;
        public GetWeaponDto weapon {get; set;}
        public List<GetSkillDto> skills {get; set;}
    }
}