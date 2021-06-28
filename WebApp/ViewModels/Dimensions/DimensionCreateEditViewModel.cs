
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Dimensions
{
    /// <summary>
    /// Dimensions create and edit viewmodel
    /// </summary>
    public class DimensionCreateEditViewModel
    {
       /// <summary>
       /// BLL.App.DTO.Dimensions 
       /// </summary>
       public BLL.App.DTO.Dimensions Dimensions { get; set; } = default!;

       /// <summary>
       /// SelectList for Units
       /// </summary>
       public SelectList? UnitSelectList { get; set; }
    }
}