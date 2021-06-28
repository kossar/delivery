using System;
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Transports
{
    /// <summary>
    /// Transport create and edit viewmodel
    /// </summary>
    public class TransportCreateEditViewModel
    {
        /// <summary>
        /// BLL.App.DTO.Transport
        /// </summary>
        public Transport Transport { get; set; } = default!;

        /// <summary>
        /// TransportOfferId for transport
        /// </summary>
        public Guid TransportOfferId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TransportOffer? TransportOffer { get; set; } = default!;

        /// <summary>
        /// Id of transportNeed
        /// </summary>
        public Guid TransportNeedId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TransportNeed? TransportNeed { get; set; } = default!;
        /// <summary>
        /// SelectList for PickUpLocation
        /// </summary>
        public SelectList? PickUpLocationSelectList { get; set; }

        /// <summary>
        /// SelectList for TransportOffer
        /// </summary>
        public SelectList? TransportOfferSelectList { get; set; }
    }
}