using System;
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Parcels
{
    /// <summary>
    /// Parcels create and edit viewmodel
    /// </summary>
    public class ParcelCreateEditViewModel
    {
        /// <summary>
        /// BLL.App.DTO.Parel
        /// </summary>
        public Parcel Parcel { get; set; } = default!;

        /// <summary>
        /// Id for transportNeed
        /// </summary>
        public Guid TransportNeedId { get; set; }

        /// <summary>
        /// In case submitting transportNeed to transport offer
        /// </summary>
        public Guid? TransportOfferId { get; set; }

        /// <summary>
        /// Selectlist for parcel dimensions
        /// </summary>
        public SelectList? DimensionUnitSelectList { get; set; }

        /// <summary>
        /// SelectList for parcel weight Units
        /// </summary>
        public SelectList? ParcelWeightUnitSelectList { get; set; }
    }
}