using AutoMapper;

namespace BLL.App.Mappers
{
    public class DimensionMapper: BaseMapper<BLL.App.DTO.Dimensions, DAL.App.DTO.Dimensions>
    {
        public DimensionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}