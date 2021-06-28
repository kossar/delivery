using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class DimensionsMapper: BaseMapper<PublicApi.DTO.v1.Dimensions, BLL.App.DTO.Dimensions>
    {
        public DimensionsMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.Dimensions MapToBll(DimensionsAdd dimensionsAdd)
        {
            var res = new BLL.App.DTO.Dimensions()
            {
                Width = dimensionsAdd.Width,
                Height = dimensionsAdd.Height,
                Length = dimensionsAdd.Length,
                UnitId = dimensionsAdd.UnitId
            };

            return res;
        }
        
        public static Dimensions MapToPublicApi(BLL.App.DTO.Dimensions dim)
        {
            var publicApiDim = new Dimensions()
            {
                Id = dim.Id,
                Width = dim.Width,
                Height = dim.Height,
                Length = dim.Length,
                UnitId = dim.UnitId,
                UnitCode = dim!.Unit!.UnitCode
            };

            return publicApiDim;
        }
    }
}