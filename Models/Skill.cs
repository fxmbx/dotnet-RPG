using System.Collections.Generic;

namespace dotnet_RPG.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }

        public List<ChracterSkill> CharacterSkills {get;set;}
    }
}