using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class TrailerMapper: BaseMapper<DAL.App.DTO.Trailer, Domain.App.Trailer>
    {
        public TrailerMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}