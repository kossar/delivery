
using System;
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Trailers
{
    /// <summary>
    /// Trailers create and edit viewmodel
    /// </summary>
    public class TrailerCreateEditViewModel
    {
        /// <summary>
        /// BLL.App.DTO.Trailer
        /// </summary>
        public Trailer Trailer { get; set; } = default!;
        
        /// <summary>
        /// SelectList for Unit
        /// </summary>
        public SelectList? TrailerLoadWeightUnitSelectList { get; set; }

        /// <summary>
        /// SelectList for Trailer size (Dimensions.Unit)
        /// </summary>
        public SelectList? TrailerSizeSelectList { get; set; }

        /// <summary>
        /// Boolean to check if user already has trailers
        /// </summary> 
        public bool UserHasTrailers { get; set; }

        /// <summary>
        /// TransportOffer Id for updating transportoffer after choosing/saving Trailer
        /// </summary>
        public Guid? TransportOfferId { get; set; }

        /// <summary>
        /// TransportNeedId in case make transport submit => transportoffer -> add trailer -> go to transport
        /// </summary>
        public Guid? TransportNeedId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool UsePreviousTrailer { get; set; }

        

        
    }
}