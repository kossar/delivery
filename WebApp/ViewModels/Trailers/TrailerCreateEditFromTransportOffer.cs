using System;
using BLL.App.DTO;

namespace WebApp.ViewModels.Trailers
{
    /// <summary>
    /// TrailerCreateEdit viewmodel for editing Trailer from transport offer
    /// </summary>
    public class TrailerWithTransportOfferIdViewModel
    {
        /// <summary>
        /// Bll.App.DTO.Trailer
        /// </summary>
        public Trailer Trailer { get; set; } = default!;

        /// <summary>
        /// TransportOfferId
        /// </summary>
        public Guid TransportOfferId { get; set; }
    }
}