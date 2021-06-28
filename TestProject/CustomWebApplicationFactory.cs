using System;
using System.Linq;
using DAL.App.EF;
using DAL.App.EF.AppDataInit;
using Domain.App;
using Domain.App.Identity;
using Domain.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestProject
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // find the dbcontext
                var descriptor = services
                    .SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                    );
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<AppDbContext>(options =>
                {
                    // do we need unique db?
                    options.UseInMemoryDatabase(builder.GetSetting("test_database_name"));
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                var userManager = scopedServices.GetRequiredService<UserManager<Domain.App.Identity.AppUser>>();

                db.Database.EnsureCreated();

                // data is already seeded
                // if (db.ContactTypes.Any()) return;
                var user = new AppUser()
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                    Email = "tere@mail.ee",
                    UserName = "yyy",
                    FirstName = "igerk",
                    LastName = "lastname",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(user, ".Tere123").Result;
                
                
                SeedAppData(db);
                db.SaveChanges();
            });
        }
        
         public static void SeedAppData(AppDbContext ctx)
        {
            foreach (var (unitCode, unitName, unitType) in InitialData.Units)
            {
                var unit = new Unit()
                {
                    UnitName = unitName,
                    UnitCode = unitCode,
                    UnitType = unitType
                };
                ctx.Units.Add(unit);
            }
            ctx.SaveChanges();

            foreach (var (country, city, address, locationInfo)  in InitialData.Locations)
            {
                var location = new Location()
                {
                    Country = country,
                    City = city,
                    Address = address
                };
                if (locationInfo != null)
                {
                    location.LocationInfo = locationInfo;
                }
                ctx.Locations.Add(location);
            }

            ctx.SaveChanges();

            var mmUnit = ctx.Units.First(u => u!.UnitCode!.Translations!.Any(t => t.Value == "mm"));
            var meterUnit = ctx.Units.First(u => u.UnitCode!.Translations!.Any(t => t.Value == "m"));

            foreach (var (width, height,length) in InitialData.DimensionsMeters)
            {
                var dim = new Dimensions()
                {
                    Width = width,
                    Height = height,
                    Length = length,
                    UnitId = meterUnit.Id
                };
                ctx.Dimensions.Add(dim);
            }
            
            foreach (var (width, height,length) in InitialData.DimensionsMm)
            {
                var dim = new Dimensions()
                {
                    Width = width,
                    Height = height,
                    Length = length,
                    UnitId = mmUnit.Id
                };
                ctx.Dimensions.Add(dim);
            }

            foreach (var (name, forGoods)  in InitialData.VehicleTypes)
            {
                var vType = new VehicleType()
                {
                    VehicleTypeName = name,
                    ForGoods = forGoods
                };
                ctx.VehicleTypes.Add(vType);
            }

            ctx.SaveChanges();

            foreach (var(info, transportType, personCount, meta) in  InitialData.TransportNeeds)
            {
                var tNeed = new TransportNeed()
                {
                    TransportNeedInfo = info,
                    TransportType = transportType,
                    PersonCount = personCount,
                    TransportMeta = meta,
                    AppUserId = Guid.Parse("10000000-0000-0000-0000-000000000001")
                };
                ctx.TransportNeeds.Add(tNeed);
            }
            
            ctx.SaveChanges();

        }
    }
}