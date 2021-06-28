using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.TransportOffers
{
    /// <summary>
    /// TransportOffer create and edit viewmodel
    /// </summary>
    public class TransportOfferCreateEditCrudViewModel
    {
        /// <summary>
        /// BLL.App.DTO.TransportOffer
        /// </summary>
        public TransportOffer TransportOffer { get; set; } = default!;

        /// <summary>
        /// SelectList for TransportMeta
        /// </summary>
        public SelectList? TransportMetaSelectList { get; set; }

        /// <summary>
        /// SelectList for Vehicle
        /// </summary>
        public SelectList? VehicleSelectList { get; set; }
        /// <summary>
        /// SelectList for Trailer
        /// </summary>
        public SelectList? TrailerSelectList { get; set; }
    }
}