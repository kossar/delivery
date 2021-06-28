using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Dimensions;

namespace WebApp.Areas.Admin.Controllers.CrudControllers
{
    /// <summary>
    /// Controller for Dimensions.
    /// </summary>
    [Authorize]
    [Area("Admin")]
    public class DimensionsCrudController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Dimensions controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public DimensionsCrudController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Dimensions
        /// <summary>
        /// For dimensions Index page. Gets all Dimensions.
        /// </summary>
        /// <returns>List of dimensions.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Dimensions.GetAllAsync();
            return View(res);
        }

        // GET: Dimensions/Details/5
        /// <summary>
        /// For details view. Gets one dimension.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Dimensions object</returns>
        [AllowAnonymous]    
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dimensions = await _bll.Dimensions.FirstOrDefaultAsync(id.Value);
            if (dimensions == null)
            {
                return NotFound();
            }

            return View(dimensions);
        }

        // GET: Dimensions/Create
        /// <summary>
        /// Gets view to create new Dimensions object.
        /// </summary>
        /// <returns>View with DimensionCreateEditViewModel</returns>
        public async Task<IActionResult> Create()
        {
            var vm = new DimensionCreateEditViewModel();
            vm.UnitSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode));
            return View(vm);
        }

        // POST: Dimensions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// For Create view. Saves new Dimensions object.
        /// </summary>
        /// <param name="vm">DimensionCreateEditViewModel</param>
        /// <returns>View with DimensionCreateEditViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DimensionCreateEditViewModel vm)
        {
            Console.WriteLine("create");
            Console.WriteLine(vm.Dimensions.Height + " L: " + vm.Dimensions.Length + " W: " + vm.Dimensions.Width);
            if (ModelState.IsValid)
            {
                Console.WriteLine("Dimensions create valid");
                _bll.Dimensions.Add(vm.Dimensions);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.UnitSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Dimensions.UnitId);
            return View(vm);
        }

        // GET: Dimensions/ModifyAndAdd/5
        /// <summary>
        /// For ModifyAndAdd view. Gets Dimensions object to edit.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with DimensionCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dimensions = await _bll.Dimensions.FirstOrDefaultAsync(id.Value);
            if (dimensions == null)
            {
                return NotFound();
            }

            var vm = new DimensionCreateEditViewModel();
            vm.Dimensions = dimensions;
            
            vm.UnitSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Dimensions.UnitId);
            return View(vm);
        }

        // POST: Dimensions/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// For edit view. Updates Dimension object.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">DimensionCreateEditViewModel</param>
        /// <returns>View with DimensionCreateEditViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DimensionCreateEditViewModel vm)
        {
            if (id != vm.Dimensions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.Dimensions.Update(vm.Dimensions);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            vm.UnitSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Dimensions.UnitId);
            return View(vm);
        }

        // GET: Dimensions/Delete/5
        /// <summary>
        /// For Delete view. Gets Dimensions object to delete.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Dimensions object.</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dimensions = await _bll.Dimensions.FirstOrDefaultAsync(id.Value);
                // .Include(d => d.Unit)
                // .FirstOrDefaultAsync(m => m.Id == id);
            if (dimensions == null)
            {
                return NotFound();
            }

            return View(dimensions);
        }

        // POST: Dimensions/Delete/5
        /// <summary>
        /// Deletes the Dimensions object.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>Redirects to Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Dimensions.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DimensionsExists(Guid id)
        {
            return await _bll.Dimensions.ExistsAsync(id);
        }
    }
}