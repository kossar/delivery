using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Locations;

namespace WebApp.Areas.Admin.Controllers.CrudControllers
{
    /// <summary>
    /// Locations controller.
    /// </summary>
    [Area("Admin")]
    public class LocationsCrudController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Locations controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public LocationsCrudController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Locations
        /// <summary>
        /// For Locations index view. Gets all Locations.
        /// </summary>
        /// <returns>View with list of Locations.</returns>
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Locations.GetAllAsync();
            return View(res);
        }

        // GET: Locations/Details/5
        /// <summary>
        /// For details view. Gets Location by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Location object.</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _bll.Locations
                .FirstOrDefaultAsync(id.Value);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        /// <summary>
        /// For create view. Prepares fields for Location.
        /// </summary>
        /// <returns>View with LocationCreateEditViewModel.</returns>
        public IActionResult Create()
        {
            var vm = new LocationCreateEditViewModel();
            return View(vm);
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// For create view. Creates new Location object.
        /// </summary>
        /// <param name="vm">LocationCreateEditViewModel</param>
        /// <returns>View with LocationCreateEditViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( LocationCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Locations.Add(vm.Location);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Locations/ModifyAndAdd/5
        /// <summary>
        /// For edit view. Gets Location to edit.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with LocationCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _bll.Locations.FirstOrDefaultAsync(id.Value);
            
            if (location == null)
            {
                return NotFound();
            }

            var vm = new LocationCreateEditViewModel();
            vm.Location = location;
            return View(vm);
        }

        // POST: Locations/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// For edit view. Updates the Location.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">LocationCreateEditViewModel</param>
        /// <returns>View with LocationCreateEditViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, LocationCreateEditViewModel vm)
        {   
            if (id != vm.Location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.Locations.Update(vm.Location);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(vm);
        }

        // GET: Locations/Delete/5
        /// <summary>
        /// For delete view. Gets the Location to delete.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with location object.</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _bll.Locations
                .FirstOrDefaultAsync(id.Value);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        /// <summary>
        /// For delete view. Deletes the Location.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>Redirects to Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Locations.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> LocationExists(Guid id)
        {
            return await _bll.Locations.ExistsAsync(id);
        }
    }
}
