using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.App;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class TrailerRepository: BaseRepository<DAL.App.DTO.Trailer, Domain.App.Trailer, AppDbContext>, ITrailerRepository
    {
        public TrailerRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new TrailerMapper(mapper))
        {
        }


        public override async Task<DAL.App.DTO.Trailer?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = query
                .Include(t => t.Dimensions)
                .ThenInclude(d => d!.Unit!.UnitCode!.Translations)
                .Include(t => t.Dimensions)
                .ThenInclude(d => d!.Unit!.UnitName!.Translations)
                .Include(t => t.Unit)
                .ThenInclude(u => u!.UnitName!.Translations)
                .Include(t => t.Unit)
                .ThenInclude( u=> u!.UnitCode!.Translations);

            var res = Mapper.Map(await  query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        public override async Task<IEnumerable<DAL.App.DTO.Trailer>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            var resQuery = query
                .Include(t => t.Dimensions)
                .Include(t => t.AppUser)
                .Include(t => t.Unit)
                .ThenInclude(u => u!.UnitName!.Translations)
                .Include(t => t.Unit)
                .ThenInclude( u=> u!.UnitCode!.Translations)
                .Select(x=> Mapper.Map(x));
            
            var res = await resQuery.ToListAsync();

            return res!;
        }

        public async Task<bool> UserHasTrailers(Guid userId)
        {
            var query = CreateQuery(userId, false);
            return await query.AnyAsync(v => v.AppUserId == userId);
        }

        public async Task<int> GetUserTrailerCount(Guid userId)
        {
            var query = CreateQuery(userId, true);
            return await query.CountAsync(v => v.AppUserId == userId);
        }
    }
}