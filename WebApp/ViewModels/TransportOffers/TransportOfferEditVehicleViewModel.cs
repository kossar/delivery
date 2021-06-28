using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Vehicles;

namespace WebApp.ViewModels.TransportOffers
{
    /// <summary>
    /// ViewModel for editing Vehicle for TransportOffer
    /// </summary>
    public class TransportOfferEditVehicleViewModel
    {
        /// <summary>
        /// Vehicle view model for transportoffer if user chooses to save new vehicle
        /// </summary>
        public VehicleCreateEditViewModel VehicleVm { get; set; } = new ();

        /// <summary>
        /// Vehicle id from selectList
        /// </summary>
        public Guid? VehicleId { get; set; }
        /// <summary>
        /// SelectList for user vehicles
        /// </summary>
        public SelectList? VehicleSelectList { get; set; }

        /// <summary>
        /// True, if user has previous vehicles
        /// </summary>
        public bool HasPreviusVehicles { get; set; }

        /// <summary>
        /// If true, display vehicle selectlist, else display create new vehicle
        /// </summary>
        public bool UsePreviousVehicle { get; set; }

        /// <summary>
        /// TransportOfferId if it is edit
        /// </summary>
        public Guid? TransportOfferId { get; set; }
    }
}