using System;
using System.Collections.Generic;
using BLL.App.DTO;

namespace WebApp.ViewModels.TransportNeeds
{
    /// <summary>
    /// TransportNeed create and edit viewmodel
    /// </summary>
    public class TransportNeedCreateEditViewModel
    {

        /// <summary>
        /// BLL.App.DTO.TransportNeed
        /// </summary>
        public TransportNeed TransportNeed { get; set; } = default!;

        /// <summary>
        /// TransportOfferId, used when submitting transportneed to transpor
        /// </summary>
        public Guid? TransportOfferId { get; set; }

        /// <summary>
        /// BLL.App.DTO TransportMeta object for Transportneed
        /// </summary>
        public TransportMeta TransportMeta { get; set; } = default!;

        /// <summary>
        /// StartLocation for TransportNeed.TransportMeta
        /// </summary>
        public Location StartLocation { get; set; } = new ();

        /// <summary>
        /// DestinationLocation for TransportNeed.TransportMeta
        /// </summary>
        public Location DestinationLocation { get; set; } = new();

        /// <summary>
        /// Maps transportneed to viewmodel
        /// </summary>
        /// <param name="transportNeed"></param>
        public void MapToVm(TransportNeed transportNeed)
        {
            TransportNeed = transportNeed;
            TransportMeta = transportNeed.TransportMeta!;
            StartLocation = transportNeed.TransportMeta!.StartLocation!;
            DestinationLocation = transportNeed.TransportMeta!.DestinationLocation!;
        }

        /// <summary>
        /// Maps TransportNeed Viewmodel to BLL.App.DTO.TransportNeed
        /// </summary>
        /// <returns>BLL.App.DTO.TransportNeed</returns>
        public TransportNeed VmToBll()
        {
            TransportMeta.StartLocation = StartLocation;
            TransportMeta.DestinationLocation = DestinationLocation;
            TransportNeed.TransportMeta = TransportMeta;

            return TransportNeed;
        }



    }
}