using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.EF;
using Domain.App.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels.Identity;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Users controller
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        //private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Users controller constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public UsersController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: Users
        /// <summary>
        /// Gets all users to list
        /// </summary>
        /// <returns>List of AppUsers</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        // GET: Users/Details/5
        /// <summary>
        /// Gets AppUser by its id value
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>AppUSer</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // GET: Userss/Create
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var vm = new UserCreateViewModel();
            return View(vm);
        }

        // POST: Userss/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm">UserCreateViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await _userManager.CreateAsync(vm.User, vm.Password);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Userss/ModifyAndAdd/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid? id)
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

        // POST: Userss/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Update AppUser
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="appUser">BLL.App.DTO.Identity.AppUser</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AppUser appUser)
        {
            if (id != appUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userManager.UpdateAsync(appUser);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: Userss/Delete/5
        /// <summary>
        /// Get AppUser to delete by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: Userss/Delete/5
        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appUser = await _userManager.FindByIdAsync(id.ToString());
            if (appUser == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(appUser);
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(Guid id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }
    }
}
