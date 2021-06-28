using System;

namespace WebApp.ViewModels.Vehicles
{
    /// <summary>
    /// ViewModel for editing Vehicle from transportOffer
    /// </summary>
    public class VehicleCreateEditFromTransportOfferViewModel: VehicleCreateEditViewModel
    {
        /// <summary>
        /// TransportOfferId
        /// </summary>
        public Guid TransportOfferId { get; set; }
    }
}