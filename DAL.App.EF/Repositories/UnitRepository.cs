using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.App.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class UnitRepository: BaseRepository<DAL.App.DTO.Unit, Domain.App.Unit, AppDbContext>, IUnitRepository
    {
        public UnitRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new UnitMapper(mapper))
        {
        }

        public override async Task<Unit> RemoveAsync(Guid id, Guid userId = default)
        {
            var entity = await RepoDbSet
                .Include(l => l.UnitName)
                .ThenInclude(c => c!.Translations)
                .Include(l => l.UnitCode)
                .ThenInclude(c => c!.Translations)
                .FirstAsync(o => o.Id == id);
            
            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            
            return Mapper.Map(RepoDbContext.Units.Remove(entity!).Entity)!;
        }
        public override Unit Update(Unit entity)
        {
            var domainEntity = Mapper.Map(entity);

            var oldUnit = RepoDbContext.Units.AsNoTracking()
                .First(x => x.Id == entity.Id);

            domainEntity!.UnitNameId = oldUnit.UnitNameId;
            domainEntity.UnitCodeId = oldUnit.UnitCodeId;
            
            
            // Load the translations (will lose dal mapper content)
            domainEntity!.UnitName
                = RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.UnitNameId);

            domainEntity!.UnitCode
                = RepoDbContext.LangStrings
                    .Include(l => l.Translations)
                    .First(x => x.Id == domainEntity.UnitCodeId);

            // set the value from dal entity back to list
            domainEntity!.UnitName.SetTranslation(entity.UnitName);
            domainEntity.UnitCode.SetTranslation(entity.UnitCode);

            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            return Mapper.Map(updatedEntity)!;
        }
        public override async Task<IEnumerable<Unit>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Include(u => u.UnitName!.Translations)
                .Include(u => u.UnitCode!.Translations)
                .Select(u => Mapper.Map(u));

            var res = await resQuery.ToListAsync();
            return res!;
        }

        public override async Task<Unit?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Include(u => u.UnitName)
                .ThenInclude(n => n!.Translations)  
                .Include(u => u.UnitCode!.Translations);

            return Mapper.Map(await resQuery.FirstOrDefaultAsync(u => u.Id == id) );
        }

        public async Task<IEnumerable<Unit>> GetLengthUnits(bool noTracking = true)
        {
            var query = CreateQuery(default, noTracking);

            var resQuery = query
                .Include(u => u.UnitName!.Translations)
                .Include(u => u.UnitCode!.Translations)
                .Where(u => u.UnitType == EUnitType.Length)
                .Select(u => Mapper.Map(u));
            
            var res = await resQuery.ToListAsync();
            return res!;
        }

        public async Task<IEnumerable<Unit>> GetWeightUnits(bool noTracking = true)
        {
            var query = CreateQuery(default, noTracking);

            var resQuery = query
                .Include(u => u.UnitName!.Translations)
                .Include(u => u.UnitCode!.Translations)
                .Where(u => u.UnitType == EUnitType.Weight)
                .Select(u => Mapper.Map(u));
            
            var res = await resQuery.ToListAsync();
            return res!;
        }
    }
}