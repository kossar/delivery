using Domain.App.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Identity
{
    /// <summary>
    /// Viewmodel for adding roles to user
    /// </summary>
    public class UserAddRoleViewModel
    {
        /// <summary>
        /// AppUSer
        /// </summary>
        public AppUser User { get; set; } = default!;

        /// <summary>
        /// SelectList for AppRoles
        /// </summary>
        public SelectList? RolesSelectList { get; set; }
    }
}