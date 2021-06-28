using System;

namespace WebApp.ViewModels.Vehicles
{
    /// <summary>
    /// Viewmodel to select existing or new Vehicle 
    /// </summary>
    public class VehicleSelectExistingOrNewViewModel
    {
        /// <summary>
        /// TransportOfferId
        /// </summary>
        public Guid TransportOfferId { get; set; } = default!;
    }
}