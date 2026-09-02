using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options) // primary constructor - combines constructor into class declaration
{
    public DbSet<AppUser> Users { get; set; }
}

