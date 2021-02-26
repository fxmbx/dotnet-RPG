namespace dotnet_RPG.Dtos.Character.Weapon
{
    public class GetWeaponDto
    {
         public int Id { get; set; }
        public string Name { get; set; } 
        public int Damage { get; set; } = 100;

        public int CharacterId { get; set; }
    }
}