using System.Data;
using AddressPrishtina.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AddressPrishtina.Data;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "9e4f49fe-0786-44c6-9061-53d2aa84fab3",
            Name = "User",
            NormalizedName = "USER"
        });
        
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "6e4f49fe-0786-44c6-9061-53d2aa84fab5",
            Name = "Admin",
            NormalizedName = "ADMIN"
        });
        
        base.OnModelCreating(builder);
    }
}