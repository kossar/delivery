using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class TransportMetaRepository :
        BaseRepository<DAL.App.DTO.TransportMeta, Domain.App.TransportMeta, AppDbContext>, ITransportMetaRepository
    {
        public TransportMetaRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext,
            new TransportMetaMapper(mapper))
        {
        }

        public override async Task<DAL.App.DTO.TransportMeta?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = query
                .Include(t => t.DestinationLocation)
                .ThenInclude(d => d!.Country!.Translations)
                .Include(t => t.DestinationLocation)
                .ThenInclude(d => d!.City!.Translations)
                .Include(t => t.DestinationLocation)
                .ThenInclude(d => d!.Address!.Translations)
                .Include(t => t.DestinationLocation)
                .ThenInclude(d => d!.LocationInfo!.Translations)
                .Include(t => t.StartLocation)
                .ThenInclude(s => s!.Country!.Translations)
                .Include(t => t.StartLocation)
                .ThenInclude(s => s!.City!.Translations)
                .Include(t => t.StartLocation)
                .ThenInclude(s => s!.Address!.Translations)
                .Include(t => t.StartLocation)
                .ThenInclude(s => s!.LocationInfo!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(d => d.Id == id));

            return res;
        }

        public override async Task<IEnumerable<DAL.App.DTO.TransportMeta>> GetAllAsync(Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Include(t => t.DestinationLocation)
                .ThenInclude(d => d!.Country!.Translations)
                .Include(t => t.DestinationLocation)
                .ThenInclude(d => d!.City!.Translations)
                .Include(t => t.DestinationLocation)
                .ThenInclude(d => d!.Address!.Translations)
                .Include(t => t.DestinationLocation)
                .ThenInclude(d => d!.LocationInfo!.Translations)
                .Include(t => t.StartLocation)
                .ThenInclude(s => s!.Country!.Translations)
                .Include(t => t.StartLocation)
                .ThenInclude(s => s!.City!.Translations)
                .Include(t => t.StartLocation)
                .ThenInclude(s => s!.Address!.Translations)
                .Include(t => t.StartLocation)
                .ThenInclude(s => s!.LocationInfo!.Translations)
                .Select(x => Mapper.Map(x));

            var res = await resQuery.ToListAsync();

            return res!;
        }
    }
}