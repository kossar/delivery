using AutoMapper;

namespace DAL.App.EF.Mappers
{
    public class ParcelMapper: BaseMapper<DAL.App.DTO.Parcel, Domain.App.Parcel>
    {
        public ParcelMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}