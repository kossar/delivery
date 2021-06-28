using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Mappers;
using DAL.App.DTO;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class VehicleTypeRepository: BaseRepository<DAL.App.DTO.VehicleType, Domain.App.VehicleType, AppDbContext>, IVehicleTypeRepository
    {
        public VehicleTypeRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new VehicleTypeMapper(mapper))
        {
        }

        public override async Task<VehicleType> RemoveAsync(Guid id, Guid userId = default)
        {
            var entity = await RepoDbSet
                .Include(l => l.VehicleTypeName)
                .ThenInclude(c => c!.Translations)
                .FirstAsync(o => o.Id == id);
            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            
            return Mapper.Map(RepoDbContext.VehicleTypes.Remove(entity!).Entity)!;
        }
        public override VehicleType Update(VehicleType entity)
        {
            var domainEntity = Mapper.Map(entity);

            domainEntity!.VehicleTypeNameId = RepoDbContext.VehicleTypes.AsNoTracking()
                .First(v => v.Id == entity.Id).VehicleTypeNameId;

            // Load the translations (will lose dal mapper content)
            domainEntity!.VehicleTypeName
                = RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.VehicleTypeNameId);

            // set the value from dal entity back to list
            domainEntity!.VehicleTypeName.SetTranslation(entity.VehicleTypeName);

            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            return Mapper.Map(updatedEntity)!;
        }
        public override async Task<IEnumerable<VehicleType>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            var resQuery = query
                .Include(v => v!.VehicleTypeName!.Translations)
                .Select(v => Mapper.Map(v));

            var res = await resQuery.ToListAsync();
            return res!;
        }

        public override async Task<VehicleType?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            query = query
                .Include(v => v!.VehicleTypeName!.Translations);
            
            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));
            return res;
        }
    }
}