using System.Collections.Generic;
using BLL.App.DTO;

namespace WebApp.ViewModels.Transports
{
    /// <summary>
    /// TransportIndex view model for transport Index page
    /// </summary>
    public class TransportIndexViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Transport>? UserTransportNeedsWaitingForUser { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Transport>? UserTransportOffersWaitingForUser { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Transport>? UserPendingTransportNeedTransports { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Transport>? UserPendingTransportOfferTransports { get; set; }
    }
}
