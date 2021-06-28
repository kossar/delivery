using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using AppUser = Domain.App.Identity.AppUser;

namespace WebApp.ViewModels.Identity
{
    /// <summary>
    /// ViewModel for User creation by admin.
    /// </summary>
    public class UserCreateViewModel
    {
        /// <summary>
        /// AppUser
        /// </summary>
        public AppUser User { get; set; } = default!;
        
        /// <summary>
        /// Property for password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        /// <summary>
        /// Property for password confirmation
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}