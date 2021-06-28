

using BLL.App.DTO;

namespace WebApp.ViewModels.Locations
{
    /// <summary>
    /// Locations create and edit viewmodel
    /// </summary>
    public class LocationCreateEditViewModel
    {
        /// <summary>
        /// BLL.App.DTO.Location
        /// </summary>
        public Location Location { get; set; } = default!;
    }
}