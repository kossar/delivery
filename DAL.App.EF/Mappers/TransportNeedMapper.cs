using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class TransportNeedMapper: BaseMapper<DAL.App.DTO.TransportNeed, Domain.App.TransportNeed>
    {
        public TransportNeedMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}