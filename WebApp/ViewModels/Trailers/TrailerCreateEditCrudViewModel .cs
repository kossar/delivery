
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Trailers
{
    /// <summary>
    /// Trailers create and edit viewmodel
    /// </summary>
    public class TrailerCreateEditCrudViewModel
    {
        /// <summary>
        /// BLL.App.DTO.Trailer
        /// </summary>
        public Trailer Trailer { get; set; } = default!;

        /// <summary>
        /// SelectList for Dimensions
        /// </summary>
        public SelectList? DimensionsSelectList { get; set; }
        /// <summary>
        /// SelectList for Unit
        /// </summary>
        public SelectList? UnitsSelectList { get; set; }
    }
}