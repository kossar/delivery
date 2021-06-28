using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Trailers
{
    /// <summary>
    /// ViewModel for choosing Trailer to transportOffer from select list
    /// </summary>
    public class TrailerFromSelectListViewModel
    {
        /// <summary>
        /// SelectList for userTrailers
        /// </summary>
        public SelectList? TrailerSelectList { get; set; }
        
        /// <summary>
        /// TransportOffer Id for updating transportoffer after choosing/saving Trailer
        /// </summary>
        public Guid? TransportOfferId { get; set; }

        /// <summary>
        /// TransportNeed id if making transport offer
        /// </summary>
        public Guid? TransportNeedId { get; set; }
        
        /// <summary>
        /// Trailer Id for TransportOffer
        /// </summary>
        public Guid? TrailerId { get; set; }
    }
}