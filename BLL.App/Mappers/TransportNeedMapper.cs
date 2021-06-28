using AutoMapper;

namespace BLL.App.Mappers
{
    public class TransportNeedMapper: BaseMapper<BLL.App.DTO.TransportNeed, DAL.App.DTO.TransportNeed>
    {
        public TransportNeedMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}