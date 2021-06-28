using System.ComponentModel.DataAnnotations;

namespace BLL.App.DTO.Enums
{
    public enum ETransportType
    {
        [Display(Name = "All", ResourceType = typeof(Resources.BLL.App.DTO.Enums.ETransortType))]
        All = 1,
        
        [Display(Name = "PersonsOnly", ResourceType = typeof(Resources.BLL.App.DTO.Enums.ETransortType))]
        PersonsOnly = 2,
        
        [Display(Name = "ParcelsOnly", ResourceType = typeof(Resources.BLL.App.DTO.Enums.ETransortType))]
        ParcelsOnly = 3
    }
}