using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class ParcelMapper: BaseMapper<PublicApi.DTO.v1.Parcel, BLL.App.DTO.Parcel>
    {
        public ParcelMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.Parcel MapToBll(ParcelAdd parcelAdd)
        {
            var bllParcel = new BLL.App.DTO.Parcel()
            {
                ParcelInfo = parcelAdd.ParcelInfo,
                Weight = parcelAdd.Weight,
                DimensionsId = parcelAdd.DimensionsId,
                TransportNeedId = parcelAdd.TransportNeedId,
                UnitId = parcelAdd.UnitId
            };

            return bllParcel;
        }
        
        public static Parcel MapToPublicApi(BLL.App.DTO.Parcel parcel)
        {
            var publicApiParcel = new Parcel()
            {
                Id = parcel.Id,
                ParcelInfo = parcel.ParcelInfo,
                Weight = parcel.Weight,
                UnitId = parcel.UnitId,
                UnitCode = parcel!.Unit!.UnitCode,
                Dimensions = DimensionsMapper.MapToPublicApi(parcel!.Dimensions!),
            };

            return publicApiParcel;
        }
    }
}