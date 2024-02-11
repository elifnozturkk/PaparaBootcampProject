using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaparaApp.Project.API.Mapping.TenantFlat;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Payments;
using PaparaApp.Project.API.Models.UserDiscountStatuses;
using PaparaApp.Project.API.Models.Users;
using PaparaApp.Project.API.Models.Users.ApartmanManagers;
using PaparaApp.Project.API.Models.Users.Tenants;
using PaparaApp.Project.API.Models.UserTimelyPaymentDetails;

namespace PaparaApp.Project.API.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options)
        : IdentityDbContext<AppUser, AppRole, Guid>(options)
    {
        public DbSet<Flat> Flats { get; set; } = default!;
        public DbSet<TenantFlat> TenantFlats { get; set; } = default!;

        public DbSet<Payment> Payments { get; set; } = default!;
        public DbSet<UserTimelyPaymentDetail> UserTimelyPaymentDetails { get; set; } = default!;
        public DbSet<UserDiscountStatus> UserDiscountStatuses { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<AppUser>("AppUser") 
                .HasValue<ApartmanManagerUser>("Manager") 
                .HasValue<TenantUser>("Tenant");
        }


    }






}
