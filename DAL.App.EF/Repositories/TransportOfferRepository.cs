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
    public class TransportOfferRepository: BaseRepository<DAL.App.DTO.TransportOffer, Domain.App.TransportOffer, AppDbContext>, ITransportOfferRepository
    {
        public TransportOfferRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new TransportOfferMapper(mapper))
        {
        }

        public override async Task<TransportOffer> RemoveAsync(Guid id, Guid userId = default)
        {
            var entity = await RepoDbSet
                .Include(l => l.TransportOfferInfo)
                .ThenInclude(c => c!.Translations)
                .FirstAsync(o => o.Id == id);
            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            
            return Mapper.Map(RepoDbContext.TransportOffers.Remove(entity!).Entity)!;
        }
        public override TransportOffer Update(TransportOffer entity)
        {
            var domainEntity = Mapper.Map(entity);

            domainEntity!.TransportOfferInfoId = RepoDbContext.TransportOffers.AsNoTracking()
                .First(x => x.Id == entity.Id)
                .TransportOfferInfoId;
            
            if (domainEntity!.TransportOfferInfoId != null)
            {
                // Load the translations (will lose dal mapper content)
                domainEntity!.TransportOfferInfo
                    = RepoDbContext.LangStrings
                        .Include(t => t.Translations)
                        .First(x => x.Id == domainEntity.TransportOfferInfoId);
                if (entity.TransportOfferInfo != null)
                {
                    // set the value from dal entity back to list
                    domainEntity!.TransportOfferInfo.SetTranslation(entity.TransportOfferInfo);
                }else if (entity.TransportOfferInfo == null)
                {
                    RepoDbContext.Remove(domainEntity.TransportOfferInfo);
                    domainEntity!.TransportOfferInfoId = null;
                }
            }

            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            return Mapper.Map(updatedEntity)!;
        }
        
        public override async Task<DAL.App.DTO.TransportOffer?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = TransportOfferQuery(query);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(o => o.Id == id));

            return res;
        }

        public override async Task<IEnumerable<DAL.App.DTO.TransportOffer>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            
            var resQuery = query
                .Include(t => t.TransportMeta)
                .Include(t => t.TransportMeta!.StartLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.StartLocation!.City!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.Country!.Translations)
                .Include(t => t.TransportMeta!.DestinationLocation!.City!.Translations)
                .Include(v => v.Vehicle)
                .Where(t => t!.TransportMeta!.StartTime > DateTime.Now)
                .Select(x => Mapper.Map(x));

            var res = await resQuery.ToListAsync();
            return res!;
        }

        private IQueryable<Domain.App.TransportOffer> TransportOfferQuery(IQueryable<Domain.App.TransportOffer> query)
        {
            var resQuery = query.Include(t => t.AppUser)
                .Include(o => o!.TransportOfferInfo!.Translations)
                .Include(t => t.TransportMeta)
                .Include(t => t!.TransportMeta!.StartLocation!.LocationInfo!.Translations)
                .Include(t => t!.TransportMeta!.StartLocation!.Country!.Translations)
                .Include(t => t!.TransportMeta!.StartLocation!.City!.Translations)
                .Include(t => t!.TransportMeta!.StartLocation!.Address!.Translations)
                .Include(t => t!.TransportMeta!.DestinationLocation!.LocationInfo!.Translations)
                .Include(t => t!.TransportMeta!.DestinationLocation!.Country!.Translations)
                .Include(t => t!.TransportMeta!.DestinationLocation!.City!.Translations)
                .Include(t => t!.TransportMeta!.DestinationLocation!.Address!.Translations)
                .Include(t => t.Unit!.UnitCode!.Translations)
                .Include(t => t.Unit!.UnitName!.Translations)
                .Include(t => t.Vehicle)
                .ThenInclude(v => v!.Make!.Translations)
                .Include(t => t.Vehicle)
                .ThenInclude(v => v!.Model!.Translations)
                .Include(t => t.Trailer)
                .ThenInclude(trailer => trailer!.Dimensions)
                .ThenInclude(d => d!.Unit!.UnitCode!.Translations)
                .Include(t => t.Trailer)
                .ThenInclude(r => r!.Unit!.UnitCode!.Translations);

            return resQuery;
        }

        public async Task<IEnumerable<TransportOffer>> GetByCountAsync(int count, bool noTracking = true)
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

        public async Task<IEnumerable<TransportOffer>> GetUserUnFinishedTransportOffers(Guid userId, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = UserTransportOfferQuery(query)
                .Where(t => t.AppUserId == userId)
                .Select(x => Mapper.Map(x));
                
            var res = await resQuery.ToListAsync();
            return res!;
        }

        public async Task<int> GetUserUnFinishedTransportOffersCount(Guid userId, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = UserTransportOfferQuery(query).CountAsync(t => t.AppUserId == userId);

            return await resQuery;
        }

        private IQueryable<Domain.App.TransportOffer> UserTransportOfferQuery(
            IQueryable<Domain.App.TransportOffer> query)
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