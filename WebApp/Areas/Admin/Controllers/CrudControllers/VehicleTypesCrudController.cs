using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.VehicleTypes;

namespace WebApp.Areas.Admin.Controllers.CrudControllers
{
    /// <summary>
    /// Controller for vehicle types
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class VehicleTypesCrudController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// VehicleTypes controller constructor.
        /// </summary>
        /// <param name="bll"></param>
        public VehicleTypesCrudController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: VehicleTypes
        /// <summary>
        /// Index view. Gets all VehicleTypes
        /// </summary>
        /// <returns>List of VehicleType entities.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _bll.VehicleTypes.GetAllAsync());
        }

        // GET: VehicleTypes/Details/5
        /// <summary>
        /// Details view. Gets VehicleType by ID
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>VehicleType entity with details.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _bll.VehicleTypes
                .FirstOrDefaultAsync(id.Value);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // GET: VehicleTypes/Create
        /// <summary>
        /// Create view. Creates VehicleTypeCreateEditViewModel
        /// </summary>
        /// <returns>View with VehicleTypeCreateEditViewModel</returns>
        public IActionResult Create()
        {
            var vm = new VehicleTypeCreateEditViewModel();
            return View(vm);
        }

        // POST: VehicleTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create view. Saves new VehicleType
        /// </summary>
        /// <param name="vm">VehicleTypeCreateEditViewModel</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleTypeCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.VehicleTypes.Add(vm.VehicleType);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: VehicleTypes/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets Vehicle to edit by ID
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with VehicleTypeCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _bll.VehicleTypes.FirstOrDefaultAsync(id.Value);
            if (vehicleType == null)
            {
                return NotFound();
            }

            var vm = new VehicleTypeCreateEditViewModel();
            vm.VehicleType = vehicleType;
            return View(vm);
        }

        // POST: VehicleTypes/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// ModifyAndAdd view. Updates edited VehicleType
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">VehicleTypeCreateEditViewModel</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, VehicleTypeCreateEditViewModel vm)
        {
            if (id != vm.VehicleType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.VehicleTypes.Update(vm.VehicleType);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: VehicleTypes/Delete/5
        /// <summary>
        /// Delete view. Gets VehicleType to delete by id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with VehicleType object</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _bll.VehicleTypes
                .FirstOrDefaultAsync(id.Value);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // POST: VehicleTypes/Delete/5
        /// <summary>
        /// Delete view. Deletes the VehicleType by id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.VehicleTypes.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> VehicleTypeExists(Guid id)
        {
            return await _bll.VehicleTypes.ExistsAsync(id);
        }
    }
}
