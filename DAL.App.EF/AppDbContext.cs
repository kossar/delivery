using System;
using System.Linq;
using DAL.Base.EF;
using Domain;
using Domain.App;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF
{
    public class AppDbContext : BaseDbContext<AppUser, AppRole, AppUserRole, IdentityUserClaim<Guid>,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>, AppLangString, AppTranslation>
    {
        //public DbSet<AppUserOrganisation> AppUserOrganisations { get; set; } = default!;
        public DbSet<Dimensions> Dimensions { get; set; } = default!;
        public DbSet<Location> Locations { get; set; } = default!;
        //public DbSet<Organisation> Organisations { get; set; } = default!;
        public DbSet<Parcel> Parcels { get; set; } = default!;
        public DbSet<Trailer> Trailers { get; set; } = default!;
        public DbSet<Transport> Transports { get; set; } = default!;
        public DbSet<TransportMeta> TransportMetas { get; set; } = default!;
        public DbSet<TransportNeed> TransportNeeds { get; set; } = default!;
        public DbSet<TransportOffer> TransportOffers { get; set; } = default!;
        public DbSet<Unit> Units { get; set; } = default!;
        public DbSet<Vehicle> Vehicles { get; set; } = default!;
        public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
        
        
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // this will remove cascade delete globally and whatever else is done in path DbContext/IdentityDbContext/BaseDbContext
            base.OnModelCreating(builder);
            // do not allow EF to create multiple FK-s, use existing LangStringId
            builder.Entity<AppTranslation>()
                .HasOne(x => x.LangString)
                .WithMany(x => x!.Translations)
                .HasForeignKey(x => x.LangStringId);

            // builder.Entity<Organisation>()
            //     .HasMany(o => o.AppUserOrganisations)
            //     .WithOne(o => o.Organisation!)
            //     .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TransportMeta>()
                .HasOne(d => d.StartLocation)
                .WithMany(l => l!.StartLocations )
                .HasForeignKey(d => d.StartLocationId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<TransportMeta>()
                .HasOne(d => d.DestinationLocation)
                .WithMany(c => c!.DestinationLocations)
                .HasForeignKey(d => d.DestinationLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vehicle>()
                .HasOne(v => v.Make)
                .WithOne(l => l!.Make!);

            // builder.Entity<AppLangString>()
            //     .HasOne(l => l.Make)
            //     .WithOne(v => v!.Make!)
            //     .HasForeignKey(v => v.MakeId);
            

            builder.Entity<TransportOffer>()
                .Property(t => t.Price)
                .HasPrecision(10, 2);
            
            builder.Entity<Transport>()
                .Property(t => t.FinalPrice)
                .HasPrecision(10, 2);
            
            builder.Entity<Dimensions>()
                .Property(t => t.Width)
                .HasPrecision(10, 2);
            
            builder.Entity<Dimensions>()
                .Property(t => t.Height)
                .HasPrecision(10, 2);
            
            builder.Entity<Dimensions>()
                .Property(t => t.Length)
                .HasPrecision(10, 2);

            builder
                .Entity<TransportOffer>()
                .HasOne(o => o.TransportMeta)
                .WithOne(m => m!.TransportOffer!)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
    }
}