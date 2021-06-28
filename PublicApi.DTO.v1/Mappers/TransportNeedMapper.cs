using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class TransportNeedMapper: BaseMapper<PublicApi.DTO.v1.TransportNeed, BLL.App.DTO.TransportNeed>
    {
        public TransportNeedMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.TransportNeed MapToBll(TransportNeedAdd transportNeedAdd)
        {
            var bllTransportNeed = new BLL.App.DTO.TransportNeed()
            {
                TransportNeedInfo = transportNeedAdd.TransportNeedInfo,
                PersonCount = transportNeedAdd.PersonCount,
                TransportMetaId = transportNeedAdd.TransportMetaId,
                TransportType = (BLL.App.DTO.Enums.ETransportType) transportNeedAdd.TransportType
            };
            return bllTransportNeed;
        }
    }
}