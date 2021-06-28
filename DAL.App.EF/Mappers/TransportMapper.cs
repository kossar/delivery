using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class TransportMapper: BaseMapper<DAL.App.DTO.Transport, Domain.App.Transport>
    {
        public TransportMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}