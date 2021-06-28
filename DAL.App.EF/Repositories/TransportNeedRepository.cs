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
    public class TransportNeedRepository: BaseRepository<DAL.App.DTO.TransportNeed, Domain.App.TransportNeed, AppDbContext>, ITransportNeedRepository
    {
        public TransportNeedRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new TransportNeedMapper(mapper))
        {
        }

        public override async Task<TransportNeed> RemoveAsync(Guid id, Guid userId = default)
        {
            var entity = await RepoDbSet
                .Include(l => l.TransportNeedInfo)
                .ThenInclude(c => c!.Translations)
                .FirstAsync(o => o.Id == id);
            
            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            
            return Mapper.Map(RepoDbContext.TransportNeeds.Remove(entity!).Entity)!;
        }
        public override TransportNeed Update(TransportNeed entity)
        {
            var domainEntity = Mapper.Map(entity);

            
            domainEntity!.TransportNeedInfoId = RepoDbContext.TransportNeeds.AsNoTracking()
                .First(x => x.Id == entity.Id)
                .TransportNeedInfoId;
            
            if (domainEntity!.TransportNeedInfoId != null)
            {
                // Load the translations (will lose dal mapper content)
                domainEntity!.TransportNeedInfo
                    = RepoDbContext.LangStrings
                        .Include(t => t.Translations)
                        .First(x => x.Id == domainEntity.TransportNeedInfoId);
                if (entity.TransportNeedInfo != null)
                {
                    // set the value from dal entity back to list
                    domainEntity!.TransportNeedInfo.SetTranslation(entity.TransportNeedInfo);
                }else if (entity.TransportNeedInfo == null)
                {
                    RepoDbContext.Remove(domainEntity.TransportNeedInfo);
                    domainEntity!.TransportNeedInfoId = null;
                }
            }

            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            return Mapper.Map(updatedEntity)!;
        }
        public override async Task<DAL.App.DTO.TransportNeed?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = TransportNeedQuery(query)
                .Include(t => t.Parcels)
                .ThenInclude(p => p.Dimensions)
                .ThenInclude(d => d!.Unit!.UnitCode!.Translations)
                .Include(t => t.Parcels)
                .ThenInclude(p => p.Unit!.UnitCode!.Translations);
            
            

            var res = Mapper.Map(await query.FirstOrDefaultAsync(t => t.Id == id));

            return res;
        }

        public override async Task<IEnumerable<DAL.App.DTO.TransportNeed>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            
            var resQuery = query.Include(t => t.TransportMeta)
                .Include(t => t.TransportMeta!.StartLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.StartLocation!.City!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.City!.Translations)
                .Where(t => t!.TransportMeta!.StartTime > DateTime.Now)
                .Select(x => Mapper.Map(x));

            var res = await resQuery.ToListAsync();
            return res!;
        }

        public async Task<TransportNeed?> GetWithParcelIds(Guid id, bool noTracking = true)
        {
            var query = CreateQuery(default, noTracking);

            query = TransportNeedQuery(query)
                .Include(t => t.Parcels);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(t => t.Id == id));
            var parcelIds = res!.Parcels!.Select(parcel => parcel.Id).ToList();

            res.ParcelIds = parcelIds;
            return res;
        }

        public async Task<IEnumerable<DAL.App.DTO.TransportNeed>> GetByCountAsync(int count, bool noTracking)
        {
            var query = CreateQuery(default, noTracking);

            var resQuery = query
                .Include(t => t.TransportMeta)
                .Include(t => t.TransportMeta!.StartLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.StartLocation!.City!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.City!.Translations)
                .Where(t => t!.TransportMeta!.StartTime > DateTime.Now && !t.Transports!.Any(c => c.TransportStatus == ETransportStatus.OnDelivery || c.TransportStatus == ETransportStatus.Accepted))
                .OrderBy(x => x!.TransportMeta!.StartTime)
                .Take(count)
                .Select(x => Mapper.Map(x));
            
            var res = await resQuery.ToListAsync();
            return res!;

        }

        public async Task<IEnumerable<TransportNeed>> GetUserUnFinishedTransportNeeds(Guid userId, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = UserTransportNeedQuery(query)
                .Where(t => t.AppUserId == userId)
                .Select(x => Mapper.Map(x));
                
            var res = await resQuery.ToListAsync();
            return res!;
        }

        private IQueryable<Domain.App.TransportNeed> TransportNeedQuery(IQueryable<Domain.App.TransportNeed> query)
        {
            var resQuery = query
                .Include(t => t!.TransportNeedInfo!.Translations)
                .Include(t => t.AppUser)
                .Include(t => t.TransportMeta)
                .Include(t => t.TransportMeta!.StartLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.StartLocation!.City!.Translations)
                .Include(t => t.TransportMeta!.StartLocation!.Address!.Translations)
                .Include(t => t.TransportMeta!.StartLocation!.LocationInfo!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.City!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.Address!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.LocationInfo!.Translations);

            return resQuery;
        }
        private IQueryable<Domain.App.TransportNeed> UserTransportNeedQuery(
            IQueryable<Domain.App.TransportNeed> query)
        {
            var resQuery = query
                .Include(t => t.TransportMeta)
                .Include(t => t.TransportMeta!.StartLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.StartLocation!.City!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.City!.Translations)
                .Where(t => t!.TransportMeta!.StartTime > DateTime.Now && t.Transports == null ||
                            t!.Transports!.Count == 0
                            || t.Transports.Any(x =>
                                x.TransportStatus != ETransportStatus.Accepted
                                || x.TransportStatus != ETransportStatus.Delivered
                                || x.TransportStatus != ETransportStatus.OnDelivery));
            return resQuery;
        }
        
    }
}