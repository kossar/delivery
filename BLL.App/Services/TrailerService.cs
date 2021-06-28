using System;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class TrailerService: BaseEntityService<IAppUnitOfWork, ITrailerRepository, BLLAppDTO.Trailer, DALAppDTO.Trailer>, ITrailerService
    {
        public TrailerService(IAppUnitOfWork serviceUow, ITrailerRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new TrailerMapper(mapper))
        {
        }


        public async Task<BLLAppDTO.Trailer?> AddWithTransportOffer(BLLAppDTO.Trailer trailer, Guid userid, Guid? transportOfferId, bool noTracking = true)
        {
            var addedTrailer = ServiceUow.Trailers.Add(Mapper.Map(trailer)!);
            if (transportOfferId != null)
            {
                var transportOffer = await ServiceUow.TransportOffers.FirstOrDefaultAsync(transportOfferId.Value, userid);
                if (transportOffer != null)
                {
                    transportOffer.Trailer = null;
                    transportOffer.TrailerId = addedTrailer.Id;
                    ServiceUow.TransportOffers.Update(transportOffer);
                }
            }

            return Mapper.Map(addedTrailer);
        }

        public async Task<bool> UserHasTrailers(Guid userId)
        {
            return await ServiceRepository.UserHasTrailers(userId);
        }

        public Task<int> GetUserTrailerCount(Guid userId)
        {
            return ServiceRepository.GetUserTrailerCount(userId);
        }
    }
}