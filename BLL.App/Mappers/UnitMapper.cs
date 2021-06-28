using AutoMapper;

namespace BLL.App.Mappers
{
    public class UnitMapper: BaseMapper<BLL.App.DTO.Unit, DAL.App.DTO.Unit>
    {
        public UnitMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}