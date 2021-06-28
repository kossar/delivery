using BLL.App.DTO;

namespace WebApp.ViewModels.VehicleTypes
{
    /// <summary>
    /// VehicleType create and edit viewmodel
    /// </summary>
    public class VehicleTypeCreateEditViewModel
    {
        /// <summary>
        /// BLL.App.DTO.VehicleType
        /// </summary>
        public VehicleType VehicleType { get; set; } = default!;
    }
}