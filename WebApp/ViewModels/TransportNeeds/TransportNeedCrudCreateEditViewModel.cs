using System.Collections.Generic;
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Parcels;

namespace WebApp.ViewModels.TransportNeeds
{
    /// <summary>
    /// TransportNeed create and edit viewmodel
    /// </summary>
    public class TransportNeedCrudCreateEditViewModel
    {

        /// <summary>
        /// BLL.App.DTO.TransportNeed
        /// </summary>
        public TransportNeed TransportNeed { get; set; } = default!;

        /// <summary>
        /// SelectList for transportMeta
        /// </summary>
        public SelectList TransportMetaSelectList { get; set; } = default!;
    }
}