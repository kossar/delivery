using AutoMapper;

namespace BLL.App.Mappers
{
    public class TrailerMapper: BaseMapper<BLL.App.DTO.Trailer, DAL.App.DTO.Trailer>
    {
        public TrailerMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}