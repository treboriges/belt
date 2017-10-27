using Microsoft.EntityFrameworkCore;
 
            namespace belt.Models
            {
                public class beltContext : DbContext
                {
                    // base() calls the parent class' constructor passing the "options" parameter along
                    public beltContext(DbContextOptions<beltContext> options) : base(options) { }
                    //the links between our DB and our models
                    // public DbSet<Person> Persons {get;set;}
                    // public DbSet<Wedding> Weddings {get;set;}
                    // public DbSet<Attendee> Attendees {get;set;}
                    public DbSet<User> users {get;set;}
                    public DbSet<Post> posts {get;set;}
                    public DbSet<Like> likes {get;set;}
                }
            }