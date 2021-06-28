using System.ComponentModel.DataAnnotations;

namespace BLL.App.DTO.Enums
{
    public enum ETransportStatus
    {
        [Display(Name = "Submitted", ResourceType = typeof(Resources.BLL.App.DTO.Enums.ETransportStatus))]
        Submitted = 1,
        
        [Display(Name = "Accepted", ResourceType = typeof(Resources.BLL.App.DTO.Enums.ETransportStatus))]
        Accepted = 2,
        
        [Display(Name = "Rejected", ResourceType = typeof(Resources.BLL.App.DTO.Enums.ETransportStatus))]
        Rejected = 3,
        
        [Display(Name = "Canceled", ResourceType = typeof(Resources.BLL.App.DTO.Enums.ETransportStatus))]
        Canceled = 4,
        
        [Display(Name = "OnDelivery", ResourceType = typeof(Resources.BLL.App.DTO.Enums.ETransportStatus))]
        OnDelivery = 5,
        
        [Display(Name = "Delivered", ResourceType = typeof(Resources.BLL.App.DTO.Enums.ETransportStatus))]
        Delivered = 6
        
    }
}