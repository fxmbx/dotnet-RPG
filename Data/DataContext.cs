using dotnet_RPG.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_RPG.Data
{
    public class DataContext : DbContext
    {
       public DataContext(DbContextOptions<DataContext> options) : base(options)
       {  
       }
       public DbSet<Character> characters {get; set; }

       public DbSet<User> users {get; set;}

       public DbSet<Weapon> weapons {get;set;}

       public DbSet<Skill> skills {get;set;}
       public DbSet<ChracterSkill> chracterSkills { get; set; }
       protected override void OnModelCreating(ModelBuilder modelBuilder){
           modelBuilder.Entity<ChracterSkill>().HasKey(cs=> new {cs.CharacterId, cs.SkillId});
       }
    }
}