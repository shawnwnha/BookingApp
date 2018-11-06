using Microsoft.EntityFrameworkCore;

namespace bookingApp.Models{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions<HomeContext> options) : base(options) { }
        public DbSet<User> users {get;set;} 
        public DbSet<Event> events {get;set;} 
        public DbSet<Joint> joint {get;set;} 
    }
}