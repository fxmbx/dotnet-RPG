namespace dotnet_RPG.Models
{
    public class ChracterSkill
    {
        public Character character { get; set; }
        public int CharacterId { get; set; }

        public int SkillId { get; set; }
        public Skill skill { get; set; }
    }
}