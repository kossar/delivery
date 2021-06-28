using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class TransportOfferMapper: BaseMapper<DAL.App.DTO.TransportOffer, Domain.App.TransportOffer>
    {
        public TransportOfferMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}