

using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Vehicles
{
    /// <summary>
    /// Vehicles create and edit viewmodel
    /// </summary>
    public class VehicleCreateEditViewModel
    {
        /// <summary>
        /// BLL.App.DTO.Vehicle
        /// </summary>
        public Vehicle Vehicle { get; set; } = default!;

        /// <summary>
        /// SelectList for Vehicle
        /// </summary>
        public SelectList? VehicleTypeSelectList { get; set; }
    }
}