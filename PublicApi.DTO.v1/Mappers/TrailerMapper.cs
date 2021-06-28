using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class TrailerMapper: BaseMapper<PublicApi.DTO.v1.Trailer, BLL.App.DTO.Trailer>
    {
        public TrailerMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.Trailer MapToBll(TrailerAdd trailerAdd)
        {
            var bllTrailer = new BLL.App.DTO.Trailer()
            {
                LoadCapacity = trailerAdd.LoadCapacity,
                RegNr = trailerAdd.RegNr,
                UnitId = trailerAdd.UnitId,
                DimensionsId = trailerAdd.DimensionsId
            };

            return bllTrailer;
        }
    }
}