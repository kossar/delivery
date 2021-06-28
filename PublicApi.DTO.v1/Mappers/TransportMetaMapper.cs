using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class TransportMetaMapper: BaseMapper<PublicApi.DTO.v1.TransportMeta, BLL.App.DTO.TransportMeta>
    {
        public TransportMetaMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.TransportMeta MapToBll(TransportMetaAdd transportMetaAdd)
        {
            return new BLL.App.DTO.TransportMeta()
            {
                StartTime = transportMetaAdd.StartTime,
                DestinationLocationId = transportMetaAdd.DestinationLocationId,
                StartLocationId = transportMetaAdd.StartLocationId
            };
        }
    }
}