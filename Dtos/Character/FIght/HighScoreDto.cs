namespace dotnet_RPG.Dtos.Character.FIght
{
    public class HighScoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Fights{get; set;}
        public int Defeats { get; set; }
        public int Victories { get; set; }
    }
}