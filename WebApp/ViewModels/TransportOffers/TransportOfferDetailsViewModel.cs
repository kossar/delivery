using Resources.BLL.App.DTO;
using TransportOffer = BLL.App.DTO.TransportOffer;

namespace WebApp.ViewModels.TransportOffers
{
    /// <summary>
    /// View model for TransportOffer Details
    /// </summary>
    public class TransportOfferDetailsViewModel
    {
        /// <summary>
        /// Bll.App.DTO.TransportOffer
        /// </summary>
        public TransportOffer TransportOffer { get; set; } = default!;

        /// <summary>
        /// Count of User Vehicles
        /// </summary>
        public int UserVehiclesCount { get; set; }

        
        /// <summary>
        /// Count of User Trailers
        /// </summary>
        public int UserTrailersCount { get; set; }
    }
}