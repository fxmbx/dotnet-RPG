namespace dotnet_RPG.Dtos.Character.FIght
{
    public class AttackResultDto
    {
        public string Attacker { get; set; }
        public string Opponent { get; set; }
        public int AttackerHP { get; set; }
        public int OppnentHP { get; set; }
        public int Damage { get; set; }
    }
}