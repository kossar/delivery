using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Units
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitCreateViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [MaxLength(8)]
        public string UnitCode { get; set; } = default!;
        
        /// <summary>
        /// 
        /// </summary>
        [MaxLength(32)]
        public string UnitName { get; set; } = default!;
    }
}