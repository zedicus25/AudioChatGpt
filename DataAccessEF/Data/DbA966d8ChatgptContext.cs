using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccesEF.Data;

public partial class DbA966d8ChatgptContext : IdentityDbContext<IdentityUser>
{
    protected readonly IConfiguration Configuration;
    public DbA966d8ChatgptContext(DbContextOptions configuration) : base(configuration)
    {
    }

    public virtual DbSet<History> History { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Responce> Responces { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       base.OnModelCreating(modelBuilder);
    }

}
