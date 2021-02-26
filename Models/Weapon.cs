namespace dotnet_RPG.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }

        public Character character {get; set;}

        public int CharacterId { get; set; }
    }
}