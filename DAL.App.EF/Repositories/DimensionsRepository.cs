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
    public class DimensionsRepository : BaseRepository<DAL.App.DTO.Dimensions, Domain.App.Dimensions, AppDbContext>, IDimensionsRepository
    {
        public DimensionsRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new DimensionMapper(mapper))
        {
        }

        public override async Task<IEnumerable<DAL.App.DTO.Dimensions>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            
            var resQuery = query
                /*.Where(d => 
                    (d.Parcels != null && d.Parcels.Any(p => p.TransportNeed != null && p.TransportNeed.AppUserId == userId)) 
                    || d.Trailers != null && d.Trailers.Any(t => t.AppUserId == userId))*/
                .Include(d => d!.Unit!.UnitCode!.Translations)
                .Include(d => d!.Unit!.UnitName!.Translations)
                .Select(x => Mapper.Map(x));
            
            var res = await resQuery.ToListAsync();

            return res!;
        }

        public override async Task<DAL.App.DTO.Dimensions?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = query.Include(d => d!.Unit!.UnitCode!.Translations)
                .Include(d => d!.Unit!.UnitName!.Translations);
            var res = Mapper.Map(await query.FirstOrDefaultAsync(d => d.Id == id));

            return res;
        }
    }
}