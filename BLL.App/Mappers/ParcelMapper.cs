using AutoMapper;

namespace BLL.App.Mappers
{
    public class ParcelMapper: BaseMapper<BLL.App.DTO.Parcel, DAL.App.DTO.Parcel>
    {
        public ParcelMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}