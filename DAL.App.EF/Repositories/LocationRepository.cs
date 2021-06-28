using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using AutoMapper;
using Domain.App;
using Microsoft.EntityFrameworkCore;
using Location = DAL.App.DTO.Location;

namespace DAL.App.EF.Repositories
{
    public class LocationRepository : BaseRepository<DAL.App.DTO.Location, Domain.App.Location, AppDbContext>,
        ILocationRepository
    {
        public LocationRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new LocationMapper(mapper))
        {
        }

        public override async Task<Location> RemoveAsync(Guid id, Guid userId = default)
        {
            //var entity = Mapper.Map(await FirstOrDefaultAsync(id, userId));
            var entity = await RepoDbSet
                .Include(l => l.Country)
                .ThenInclude(c => c!.Translations)
                .Include(l => l.City)
                .ThenInclude(c => c!.Translations)
                .Include(l => l.Address)
                .ThenInclude(a => a!.Translations)
                .Include(l => l.LocationInfo)
                .ThenInclude(i => i!.Translations)
                .FirstAsync(l => l.Id == id);
            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            return Mapper.Map(RepoDbContext.Locations.Remove(entity!).Entity)!;
        }

        public override Location Update(Location entity)
        {
            var domainEntity = Mapper.Map(entity);

            var oldLocation = RepoDbContext.Locations.AsNoTracking().First(x => x.Id == entity.Id);

            domainEntity!.LocationInfoId = oldLocation.LocationInfoId;
            domainEntity!.CountryId = oldLocation.CountryId;
            domainEntity!.CityId = oldLocation.CityId;
            domainEntity!.AddressId = oldLocation.AddressId;
            
            
            
            // Load the translations (will lose dal mapper content)
            domainEntity!.Country
                = RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.CountryId);

            // set the value from dal entity back to list
            domainEntity!.Country.SetTranslation(entity.Country);

            domainEntity!.City
                = RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.CityId);

            domainEntity!.City.SetTranslation(entity.City);

            domainEntity!.Address
                = RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.AddressId);

            domainEntity!.Address.SetTranslation(entity.Address);

            // Nullable info
            if (domainEntity.LocationInfoId != null)
            {
                domainEntity.LocationInfo = RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .FirstOrDefault(x => x.Id == domainEntity.LocationInfoId);

                if (entity.LocationInfo != null)
                {
                    domainEntity!.LocationInfo!.SetTranslation(entity.LocationInfo);
                }else if (entity.LocationInfo == null)
                {
                    RepoDbContext.Remove(domainEntity.LocationInfo);
                    domainEntity!.LocationInfoId = null;
                }
                
            }
            

            var updatedEntity = RepoDbContext.Update(domainEntity!).Entity;
            return Mapper.Map(updatedEntity)!;
        }

        public async Task<IEnumerable<Location>> GetAllWithAdditionalDataCount(Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Include(l => l.Country!.Translations)
                .Include(l => l!.City!.Translations)
                .Include(l => l!.Address!.Translations)
                .Include(l => l!.LocationInfo!.Translations)
                .Select(x => new Location()
                {
                    Id = x.Id,
                    Country = x.Country!.ToString(),
                    City = x.City!.Translations!.ToString()!,
                    Address = x.Address!.Translations!.ToString()!,
                    LocationInfo = x!.LocationInfo!.Translations!.ToString() ?? "",
                    TransportAdStartLocationCount = x.StartLocations!.Count,
                    TransportAdDestinationLocationCount = x.DestinationLocations!.Count,
                    PickUpLocationCount = x.Transports!.Count
                });

            var res = await resQuery.ToListAsync();

            return res!;
        }

        public override async Task<IEnumerable<Location>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Include(l => l.Country!.Translations)
                .Include(l => l.City!.Translations)
                .Include(l => l.Address!.Translations)
                .Include(l => l.LocationInfo!.Translations)
                .Select(x => Mapper.Map(x));

            var res = await resQuery.ToListAsync();

            return res!;
        }

        public override async Task<Location?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = query
                .Include(l => l.Country)
                .ThenInclude(c => c!.Translations)
                .Include(l => l.City)
                .ThenInclude(c => c!.Translations)
                .Include(l => l.Address)
                .ThenInclude(a => a!.Translations)
                .Include(l => l.LocationInfo)
                .ThenInclude(i => i!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }
    }
}