using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Role { get; set; }
    }
}