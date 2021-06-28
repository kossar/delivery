using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.App;
using Microsoft.EntityFrameworkCore;
using Vehicle = DAL.App.DTO.Vehicle;

namespace DAL.App.EF.Repositories
{
    public class VehicleRepository: BaseRepository<DAL.App.DTO.Vehicle, Domain.App.Vehicle, AppDbContext>, IVehicleRepository
    {
        public VehicleRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new VehicleMapper(mapper))
        {
        }

        public override async Task<Vehicle> RemoveAsync(Guid id, Guid userId = default)
        {
            var entity = await RepoDbSet
                .Include(v => v.Make)
                .ThenInclude(m => m!.Translations)
                .Include(v => v.Model)
                .ThenInclude(l => l!.Translations)
                .FirstAsync(o => o.Id == id);
            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            
            return Mapper.Map(RepoDbContext.Vehicles.Remove(entity!).Entity)!;
        }
        public override Vehicle Update(Vehicle entity)
        {
            var domainEntity = Mapper.Map(entity);

            var oldVehicle = RepoDbContext.Vehicles.AsNoTracking().First(v => v.Id == entity.Id);
            domainEntity!.ModelId = oldVehicle.ModelId;
            domainEntity!.MakeId = oldVehicle.MakeId;

            // Load the translations (will lose dal mapper content)
            domainEntity!.Make
                = RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.MakeId);

            domainEntity!.Model
                = RepoDbContext.LangStrings
                    .Include(v => v.Translations)
                    .First(v => v.Id == domainEntity.ModelId);

            // set the value from dal entity back to list
            domainEntity!.Make.SetTranslation(entity.Make);
            domainEntity!.Model.SetTranslation(entity.Model);

            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            return Mapper.Map(updatedEntity)!;
        }
        public override async Task<DAL.App.DTO.Vehicle?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            query = query
                .Include(v => v.AppUser)
                .Include(v => v.VehicleType)
                .ThenInclude(t => t!.VehicleTypeName!.Translations)
                .Include(v => v!.Make!.Translations)
                .Include(v => v!.Model!.Translations);
            
            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));
            return res;
        }

        public override async Task<IEnumerable<DAL.App.DTO.Vehicle>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            
            
            var resQuery = query
                .Include(v => v.AppUser)
                .Include(v => v.VehicleType)
                .ThenInclude(t => t!.VehicleTypeName!.Translations)
                .Include(v => v!.Make!.Translations)
                .Include(v => v!.Model!.Translations)
                .Select(x => Mapper.Map(x));

            var res = await resQuery.ToListAsync();
            return res!;
        }

        public async Task<bool> HasVehicles(Guid userId)
        {
            var query = CreateQuery(userId, false);
            return await query.AnyAsync(v => v.AppUserId == userId);
        }

        public async Task<int> GetUserVehiclesCount(Guid userId)
        {
            var query = CreateQuery(userId, true);
            return await query.CountAsync(v => v.AppUserId == userId);

        }
    }
}