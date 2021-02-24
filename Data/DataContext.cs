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
    }
}