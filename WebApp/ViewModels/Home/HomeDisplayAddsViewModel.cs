using System.Collections.Generic;
using BLL.App.DTO;

namespace WebApp.ViewModels.Home
{
    /// <summary>
    /// Viewmodel for displaying TransportNeeds and TransportOffers
    /// </summary>
    public class HomeDisplayAddsViewModel
    {
        /// <summary>
        /// ICollection of TransportNeeds
        /// </summary>
        public IEnumerable<TransportNeed> TransportNeeds { get; set; } = default!;

        /// <summary>
        /// ICollection of transport offers
        /// </summary>
        public IEnumerable<TransportOffer> TransportOffers { get; set; } = default!;
    }
}