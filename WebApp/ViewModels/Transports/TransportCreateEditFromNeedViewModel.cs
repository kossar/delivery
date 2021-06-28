using System;
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Transports
{
    /// <summary>
    /// 
    /// </summary>
    public class TransportCreateEditFromNeedViewModel
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
        /// Id of transportNeed
        /// </summary>
        public Guid TransportNeedId { get; set; }

        /// <summary>
        /// SelectList for TransportOffer
        /// </summary>
        public SelectList? TransportOfferSelectList { get; set; }
    }
}