using Microsoft.EntityFrameworkCore;


namespace PrekoTarabu.Server.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public virtual DbSet<WaitLister> WaitListers { get; set; }
    

    
}