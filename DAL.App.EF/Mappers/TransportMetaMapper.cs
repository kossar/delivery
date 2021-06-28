using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class TransportMetaMapper: BaseMapper<DAL.App.DTO.TransportMeta, Domain.App.TransportMeta>
    {
        public TransportMetaMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}