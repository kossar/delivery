using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Transports
{
    /// <summary>
    /// Transport create and edit viewmodel
    /// </summary>
    public class TransportCreateEditCrudViewModel
    {
        /// <summary>
        /// BLL.App.DTO.Transport
        /// </summary>
        public Transport Transport { get; set; } = default!;

        /// <summary>
        /// SelectList for PickUpLocation
        /// </summary>
        public SelectList? PickUpLocationSelectList { get; set; }
        /// <summary>
        /// SelectList for TransportNeed
        /// </summary>
        public SelectList? TransportNeedSelectList { get; set; }
        /// <summary>
        /// SelectList for TransportOffer
        /// </summary>
        public SelectList? TransportOfferSelectList { get; set; }
    }
}