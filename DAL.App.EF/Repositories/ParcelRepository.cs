using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class ParcelRepository: BaseRepository<DAL.App.DTO.Parcel, Domain.App.Parcel, AppDbContext>, IParcelRepository
    {
        public ParcelRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new ParcelMapper(mapper))
        {
        }

        public override async Task<Parcel> RemoveAsync(Guid id, Guid userId = default)
        {
            var entity = await RepoDbSet
                .Include(l => l.ParcelInfo)
                .ThenInclude(c => c!.Translations)
                .FirstAsync(o => o.Id == id);
            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            
            return Mapper.Map(RepoDbContext.Parcels.Remove(entity!).Entity)!;
        }
        public override Parcel Update(Parcel entity)
        {
            var domainEntity = Mapper.Map(entity);

            domainEntity!.ParcelInfoId =  RepoDbContext.Parcels.AsNoTracking()
                .First(x => x.Id == entity.Id)
                .ParcelInfoId;
            
            // Load the translations (will lose dal mapper content)
            if (domainEntity!.ParcelInfoId != null)
            {
                domainEntity!.ParcelInfo
                    = RepoDbContext.LangStrings
                        .Include(t => t.Translations)
                        .First(x => x.Id == domainEntity.ParcelInfoId);
                if (entity.ParcelInfo != null)
                {
                    // set the value from dal entity back to list
                    domainEntity!.ParcelInfo.SetTranslation(entity.ParcelInfo);
                }else if (entity.ParcelInfo == null)
                {
                    RepoDbContext.Remove(domainEntity.ParcelInfo);
                    domainEntity!.ParcelInfoId = null;
                }
            }

            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            return Mapper.Map(updatedEntity)!;
        }
        

        public override async Task<IEnumerable<DAL.App.DTO.Parcel>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = ParcelQuery(query)
                .Where(p => p.TransportNeed != null && p.TransportNeed.AppUserId == userId)
                .Select(x=> Mapper.Map(x));

            var res = await resQuery.ToListAsync();
    
            return res!;
        }

        public override async Task<DAL.App.DTO.Parcel?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = ParcelQuery(query);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }
        
        private IQueryable<Domain.App.Parcel> ParcelQuery(IQueryable<Domain.App.Parcel> query)
        {
            var resQuery = query
                .Include(p => p.ParcelInfo!.Translations)
                .Include(p => p.Dimensions)
                .ThenInclude(d => d!.Unit!.UnitCode!.Translations)
                .Include(p => p.Dimensions)
                .ThenInclude(d => d!.Unit!.UnitName!.Translations)
                .Include(p => p.Unit)
                .ThenInclude(u => u!.UnitCode!.Translations)
                .Include(p => p.Unit)
                .ThenInclude(u => u!.UnitName!.Translations);

            return resQuery;
        }
        
        
    }
}