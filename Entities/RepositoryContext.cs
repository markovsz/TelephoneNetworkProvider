using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AdministratorMessage> AdministratorMessages { get; set; }
        public DbSet<Call> Calls { get; set; }

        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<Customer>()
                .HasOne(c => c.user)
                .WithOne(u => u.RelatedCustomer)
                .HasForeignKey("");

            modelBuilder.Entity<Customer>()
                .Property(c => c.MoneyBalance)
                .HasColumnType("money")
                .HasPrecision(10);

            modelBuilder.Entity<Call>()
                .HasOne(c => c.Caller)
                .WithOne(c => c.);*/


            modelBuilder.Entity<Call>()
                .HasOne(c => c.Caller)
                .WithMany(c => c.InitiatedCalls)
                .HasForeignKey(c => c.CallerId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            modelBuilder.Entity<Call>()
                .HasOne(c => c.CalledBy)
                .WithMany(c => c.ReceivedCalls)
                .HasForeignKey(c => c.CalledById)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
            

            modelBuilder.Entity<Customer>()
                .Property(c => c.MoneyBalance)
                .HasColumnType("money");

            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
            //modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}
