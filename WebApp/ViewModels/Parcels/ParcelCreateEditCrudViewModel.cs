using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Parcels
{
    /// <summary>
    /// Parcels create and edit viewmodel
    /// </summary>
    public class ParcelCreateEditCrudViewModel
    {
        /// <summary>
        /// BLL.App.DTO.Parel
        /// </summary>
        public Parcel Parcel { get; set; } = default!;

        /// <summary>
        /// SelectList for Dimensions
        /// </summary>
        public SelectList? DimensionsSelectList { get; set; }
        
        /// <summary>
        /// SelectList for TransportNeeds
        /// </summary>
        public SelectList? TransportNeedsSelectList { get; set; }
        
        /// <summary>
        /// SelectList for parcel weight Units
        /// </summary>
        public SelectList? UnitsSelectList { get; set; }
    }
}