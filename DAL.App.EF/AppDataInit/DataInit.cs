using System;
using System.Collections.Generic;
using System.Linq;
using Domain.App;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.App.EF.AppDataInit
{
    public static class DataInit
    {
        public static void DropDatabase(AppDbContext ctx, ILogger logger)
        {
            logger.LogInformation("DropDatabase");
            ctx.Database.EnsureDeleted();
        }

        public static void MigrateDatabase(AppDbContext ctx, ILogger logger)
        {
            ctx.Database.Migrate();
        }

        public static void SeedAppData(AppDbContext ctx, ILogger logger)
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

        }

        public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ILogger logger)
        {
            logger.LogInformation("SeedIdentity");
            foreach (var (roleName, roleDisplayName, id) in InitialData.Roles)
            {
                var role = roleManager.FindByNameAsync(roleName).Result;
                if (role == null)
                {
                    role = new AppRole()
                    {
                        Id = id,
                        Name = roleName,
                        DisplayName = roleDisplayName
                    };

                    var result = roleManager.CreateAsync(role).Result;
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("Role creation failed!");
                    }
                    logger.LogInformation("Seeded role {Role}", roleName);
                }
            }


            foreach (var userInfo in InitialData.Users)
            {
                var user = userManager.FindByEmailAsync(userInfo.name).Result;
                if (user == null)
                {
                    user = new AppUser()
                    {
                        Id = userInfo.id,
                        Email = userInfo.name,
                        UserName = userInfo.name,
                        FirstName = userInfo.firstName,
                        LastName = userInfo.lastName,
                        EmailConfirmed = true
                    };
                    var result = userManager.CreateAsync(user, userInfo.password).Result;
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("User creation failed!");
                    }
                    logger.LogInformation("Seeded user {User}", userInfo.name);
                }

                if (userInfo.role != "")
                {
                    var roleResult = userManager.AddToRoleAsync(user, userInfo.role).Result;
                }
            }
        }
    }
}