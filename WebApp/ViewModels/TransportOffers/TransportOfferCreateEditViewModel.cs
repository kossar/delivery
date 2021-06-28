using System;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Vehicles;

namespace WebApp.ViewModels.TransportOffers
{
    /// <summary>
    /// TransportOffer create and edit viewmodel
    /// </summary>
    public class TransportOfferCreateEditViewModel
    {
        /// <summary>
        /// BLL.App.DTO.TransportOffer
        /// </summary>
        public TransportOffer TransportOffer { get; set; } = default!;

        /// <summary>
        /// TransportMeta for transportOffer
        /// </summary>
        public TransportMeta TransportMeta { get; set; } = default!;

        /// <summary>
        /// StartLocation for TransportMeta
        /// </summary>
        public Location StartLocation { get; set; } = default!;

        /// <summary>
        /// Destination Location for TransportMEta
        /// </summary>
        public Location DestinationLocation { get; set; } = default!;

        /// <summary>
        /// VehicleId for TransportOffer
        /// </summary>
        public Guid VehicleId { get; set; }

        /// <summary>
        /// Use trailer
        /// </summary>
        [Required]
        [Display(Name = "_UseTrailer", ResourceType = typeof(Resources.ViewModels.TransportOfferCreateEditViewModel))]
        public bool? UseTrailer { get; set; }

        /// <summary>
        /// If true, then view has selectbox to select from existing trailer
        /// </summary>
        public SelectList? WeightUnitSelectList { get; set; }

        /// <summary>
        /// TransportNeedId. Used in case when user is making Transport offering 
        /// </summary>
        public Guid? TransportNeedId { get; set; }

        /// <summary>
        /// Map vm to bll
        /// </summary>
        /// <returns></returns>
        public TransportOffer VmToBll()
        {
            TransportMeta.StartLocation = StartLocation;
            TransportMeta.DestinationLocation = DestinationLocation;
            TransportOffer.TransportMeta = TransportMeta;
            TransportOffer.VehicleId = VehicleId;

            return TransportOffer;
        }

        /// <summary>
        /// Bll.App.DTO.TransportOffer to vm
        /// </summary>
        public void BllToVm(TransportOffer offer)
        {
            VehicleId = offer.VehicleId;
            TransportOffer = offer;
            TransportMeta = offer.TransportMeta!;
            StartLocation = offer.TransportMeta!.StartLocation!;
            DestinationLocation = offer.TransportMeta!.DestinationLocation!;

        }
    }
}