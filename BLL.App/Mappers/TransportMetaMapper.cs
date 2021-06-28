using AutoMapper;

namespace BLL.App.Mappers
{
    public class TransportMetaMapper: BaseMapper<BLL.App.DTO.TransportMeta, DAL.App.DTO.TransportMeta>
    {
        public TransportMetaMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}