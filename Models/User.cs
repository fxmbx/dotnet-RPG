using System.Collections.Generic;

namespace dotnet_RPG.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public List<Character> characters { get; set; }
    }
}