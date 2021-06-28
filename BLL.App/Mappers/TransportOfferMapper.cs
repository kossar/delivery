using AutoMapper;

namespace BLL.App.Mappers
{
    public class TransportOfferMapper: BaseMapper<BLL.App.DTO.TransportOffer, DAL.App.DTO.TransportOffer>
    {
        public TransportOfferMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}