using AutoMapper;

namespace BLL.App.Mappers
{
    public class TransportMapper: BaseMapper<BLL.App.DTO.Transport, DAL.App.DTO.Transport>
    {
        public TransportMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}