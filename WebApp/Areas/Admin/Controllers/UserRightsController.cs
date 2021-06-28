using System;
using System.Threading.Tasks;
using Domain.App.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels.Identity;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for user roles 
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserRightsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        /// <summary>
        /// User rights controller constructor
        /// </summary>
        public UserRightsController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET
        /// <summary>
        /// Get all Users 
        /// </summary>
        /// <returns>List of AppUsers</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }
        
        /// <summary>
        /// Blocks User
        /// </summary>
        /// <param name="id"></param>
        /// <returns>AppUser</returns>
        public async Task<IActionResult> Block(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(id.ToString());
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddRole(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appUser = await _userManager.FindByIdAsync(id.ToString());
            if (appUser == null)
            {
                return NotFound();
            }

            var vm = new UserAddRoleViewModel();
            vm.User = appUser;
            
            vm.RolesSelectList = new SelectList(new SelectList(await _roleManager.Roles.ToListAsync(), nameof(AppRole.Id), nameof(AppRole.Name)));

            return View(vm);
        }
        
    }
}