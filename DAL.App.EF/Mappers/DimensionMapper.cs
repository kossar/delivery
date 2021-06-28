using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class DimensionMapper: BaseMapper<DAL.App.DTO.Dimensions, Domain.App.Dimensions>
    {
        public DimensionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}