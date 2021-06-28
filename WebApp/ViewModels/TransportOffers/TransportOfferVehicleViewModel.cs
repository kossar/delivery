using System;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Vehicles;

namespace WebApp.ViewModels.TransportOffers
{
    /// <summary>
    /// Viewmodel for selecting vehicle before adding transportOffer
    /// </summary>
    public class TransportOfferVehicleViewModel
    {
        /// <summary>
        /// Vehicle view model for transportoffer if user chooses to save new vehicle
        /// </summary>
        public VehicleCreateEditViewModel VehicleVm { get; set; } = new ();
        
        /// <summary>
        /// TransportNeedId. Used in case when user is making Transport offering 
        /// </summary>
        public Guid? TransportNeedId { get; set; }

        /// <summary>
        /// Vehicle id from selectList
        /// </summary>
        public Guid VehicleId { get; set; }
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
        
    }
}
