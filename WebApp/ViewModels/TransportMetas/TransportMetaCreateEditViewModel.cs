using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.TransportMetas
{
    /// <summary>
    /// ViewModel for TransportMeta entity
    /// </summary>
    public class TransportMetaCreateEditViewModel
    {
        /// <summary>
        /// TransportMeta object
        /// </summary>
        public TransportMeta TransportMeta { get; set; } = default!;

        /// <summary>
        /// SelectList of StartLocations
        /// </summary>
        public SelectList? StartLocationSelectList { get; set; }
        /// <summary>
        /// SelectList for Destination Locations
        /// </summary>
        public SelectList? DestinationLocationSelectList { get; set; }
    }
}