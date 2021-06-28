using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain.App.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Roles Controller
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        /// <summary>
        /// Roles controller constructor
        /// </summary>
        /// <param name="roleManager">RoleManager</param>
        public RolesController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: Admin/Roles
        /// <summary>
        /// Get all roles.
        /// </summary>
        /// <returns>List of roles</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }

        // GET: Admin/Roles/Details/5
        /// <summary>
        /// AppRole details
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>AppRole</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRole = await _roleManager.FindByIdAsync(id.ToString());
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }

        // GET: Admin/Roles/Create
        /// <summary>
        /// 
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create AppRole
        /// </summary>
        /// <param name="appRole"></param>
        /// <returns>Viw with AppRole</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( AppRole appRole)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(appRole);
                return RedirectToAction(nameof(Index));
            }
            return View(appRole);
        }

        // GET: Admin/Roles/ModifyAndAdd/5
        /// <summary>
        /// Gets AppRole to edit by id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with AppRole</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRole = await _roleManager.FindByIdAsync(id.ToString());
            if (appRole == null)
            {
                return NotFound();
            }
            return View(appRole);
        }

        // POST: Admin/Roles/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Save edited Approle
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appRole"></param>
        /// <returns>View with AppRole</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AppRole appRole)
        {
            if (id != appRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _roleManager.UpdateAsync(appRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppRoleExists(appRole.Id))
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
            return View(appRole);
        }

        // GET: Admin/Roles/Delete/5
        /// <summary>
        /// Delete AppRole
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>AppRole</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRole = await _roleManager.FindByIdAsync(id.ToString());
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }

        // POST: Admin/Roles/Delete/5
        /// <summary>
        /// Deletes AppRole
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appRole = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(appRole);
            return RedirectToAction(nameof(Index));
        }

        private bool AppRoleExists(Guid id)
        {
            return _roleManager.Roles.Any(e => e.Id == id);
        }
    }
}
