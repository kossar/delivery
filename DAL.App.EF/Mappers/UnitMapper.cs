using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class UnitMapper: BaseMapper<DAL.App.DTO.Unit, Domain.App.Unit>
    {
        public UnitMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}