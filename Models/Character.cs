using System;
using System.Collections.Generic;

namespace dotnet_RPG.Models
{
    public class Character
    {
        public int Id { get; set; }

        public string Name { get; set; } = "Frodo";

        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;

        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;

        public RpgClass Class {get; set;} = RpgClass.Knigth;
        public User users { get; set; }

        public Weapon weapon {get; set;}

        public List<ChracterSkill> CharacterSkills {get;set;}
    }
}