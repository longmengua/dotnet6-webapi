using dotnet6_webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet6_webapi.Contexts;

public class MyDatabaseContext : DbContext
{
    public DbSet<User>? Users { get; set; }
    public DbSet<Auth>? Auths { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // If you want to specify any custom configurations or table names, you can do so here.
    }
}
