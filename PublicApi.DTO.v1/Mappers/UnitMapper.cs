using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class UnitMapper: BaseMapper<PublicApi.DTO.v1.Unit, BLL.App.DTO.Unit>
    {
        public UnitMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.Unit MapToBll(UnitAdd unitAdd)
        {
            var bllUnit = new BLL.App.DTO.Unit()
            {
                UnitCode = unitAdd.UnitCode,
                UnitName = unitAdd.UnitName
            };

            return bllUnit;
        }
    }
}