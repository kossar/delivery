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
    public class TransportRepository: BaseRepository<DAL.App.DTO.Transport, Domain.App.Transport, AppDbContext>, ITransportRepository
    {
        public TransportRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new TransportMapper(mapper))
        {
        }

        public override async Task<DAL.App.DTO.Transport?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = query
                .Include(t => t.PickUpLocation)
                .Include(t => t.PickUpLocation!.Country!.Translations)
                .Include(t => t.PickUpLocation!.City!.Translations)
                .Include(t => t.PickUpLocation!.Address!.Translations)
                .Include(t => t.PickUpLocation!.LocationInfo!.Translations)
                .Where(t => t.TransportNeed!.AppUserId == userId || t.TransportOffer!.AppUserId == userId);
                // .Include(t => t.TransportNeed!.TransportNeedInfo!.Translations)
                // .Include(t => t.TransportNeed!.TransportMeta!.StartLocation!.Country!.Translations)
                // .Include(t => t.TransportNeed!.TransportMeta!.StartLocation!.City!.Translations)
                // .Include(t => t.TransportNeed!.TransportMeta!.StartLocation!.Address!.Translations)
                // .Include(t => t.TransportNeed!.TransportMeta!.StartLocation!.LocationInfo!.Translations)
                // .Include(t => t.TransportNeed!.TransportMeta!.DestinationLocation!.Country!.Translations)
                // .Include(t => t.TransportNeed!.TransportMeta!.DestinationLocation!.City!.Translations)
                // .Include(t => t.TransportNeed!.TransportMeta!.DestinationLocation!.Address!.Translations)
                // .Include(t => t.TransportNeed!.TransportMeta!.DestinationLocation!.LocationInfo!.Translations)
                // .Include(t => t.TransportOffer!.TransportOfferInfo!.Translations)
                // .Include(t => t.TransportOffer!.TransportMeta!.StartLocation!.Country!.Translations)
                // .Include(t => t.TransportOffer!.TransportMeta!.StartLocation!.City!.Translations)
                // .Include(t => t.TransportOffer!.TransportMeta!.StartLocation!.Address!.Translations)
                // .Include(t => t.TransportOffer!.TransportMeta!.StartLocation!.LocationInfo!.Translations)
                // .Include(t => t.TransportOffer!.TransportMeta!.DestinationLocation!.Country!.Translations)
                // .Include(t => t.TransportOffer!.TransportMeta!.DestinationLocation!.City!.Translations)
                // .Include(t => t.TransportOffer!.TransportMeta!.DestinationLocation!.Address!.Translations)
                // .Include(t => t.TransportOffer!.TransportMeta!.DestinationLocation!.LocationInfo!.Translations)
                // .Include(t => t.TransportOffer!.Unit!.UnitName!.Translations)
                // .Include(t => t.TransportOffer!.Unit!.UnitCode!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(t => t.Id == id));

            return res;

        }

        public override async Task<IEnumerable<DAL.App.DTO.Transport>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            
            var resQuery = query
                .Include(t => t.PickUpLocation)
                .Include(t => t.TransportNeed)
                .Include(t => t.TransportOffer)
                .Where(t => t!.TransportNeed!.AppUserId == userId || t!.TransportOffer!.AppUserId == userId)
                .Select(x => Mapper.Map(x));

            var res = await resQuery.ToListAsync();
            return res!;
        }

        public async Task<IEnumerable<Transport>> GetUserTransportNeedTransports(Guid userId, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Where(t => t.Id == t.TransportNeed!.Transports!.OrderByDescending(
                    n => n.LastUpdateTime).First().Id 
                            && t.TransportStatus != ETransportStatus.Delivered && t.TransportNeed.AppUserId == userId 
                            && t.PickUpTime > DateTime.Now
                            && !t.UpdatedByTransportOffer)
                .Include(t => t.PickUpLocation!.Country!.Translations)
                .Include(t => t.PickUpLocation!.City!.Translations)
                .Include(t => t.PickUpLocation!.Address!.Translations)
                .Include(t => t.PickUpLocation!.LocationInfo!.Translations)
                .Select(x => Mapper.Map(x));
            
            var res = await resQuery.ToListAsync();
            return res!;
        }

        public async Task<IEnumerable<Transport>> GetUserTransportOfferTransports(Guid userId, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Where(t => t.Id == t.TransportOffer!.Transports!.OrderByDescending(
                    n => n.LastUpdateTime).First().Id 
                            && t.TransportStatus != ETransportStatus.Delivered 
                            && t.TransportOffer.AppUserId == userId && t.PickUpTime > DateTime.Now
                            && t.UpdatedByTransportOffer)
                .Include(t => t.PickUpLocation!.Country!.Translations)
                .Include(t => t.PickUpLocation!.City!.Translations)
                .Include(t => t.PickUpLocation!.Address!.Translations)
                .Include(t => t.PickUpLocation!.LocationInfo!.Translations)
                .Select(x => Mapper.Map(x));
            
            var res = await resQuery.ToListAsync();
            return res!;
        }

        public async Task<IEnumerable<Transport>> GetUserTransportNeedsWaitingForUserAction(Guid userId, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Where(t => t.Id == t.TransportNeed!.Transports!.OrderByDescending(
                                n => n.LastUpdateTime).First().Id 
                            && t.TransportStatus != ETransportStatus.Delivered && t.TransportNeed.AppUserId == userId 
                            && t.PickUpTime > DateTime.Now
                            && t.UpdatedByTransportOffer)
                .Include(t => t.PickUpLocation!.Country!.Translations)
                .Include(t => t.PickUpLocation!.City!.Translations)
                .Include(t => t.PickUpLocation!.Address!.Translations)
                .Include(t => t.PickUpLocation!.LocationInfo!.Translations)
                .Select(x => Mapper.Map(x));
            
            var res = await resQuery.ToListAsync();
            return res!;
        }

        public async Task<IEnumerable<Transport>> GetUserTransportOffersWaitingForUserAction(Guid userId, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Where(t => t.Id == t.TransportOffer!.Transports!.OrderByDescending(
                                n => n.LastUpdateTime).First().Id 
                            && t.TransportStatus != ETransportStatus.Delivered 
                            && t.TransportOffer.AppUserId == userId && t.PickUpTime > DateTime.Now
                            && !t.UpdatedByTransportOffer)
                .Include(t => t.PickUpLocation!.Country!.Translations)
                .Include(t => t.PickUpLocation!.City!.Translations)
                .Include(t => t.PickUpLocation!.Address!.Translations)
                .Include(t => t.PickUpLocation!.LocationInfo!.Translations)
                .Select(x => Mapper.Map(x));
            
            var res = await resQuery.ToListAsync();
            return res!;
        }
    }
}