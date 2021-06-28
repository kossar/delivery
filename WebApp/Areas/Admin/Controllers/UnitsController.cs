using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.Units;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Units Controller
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UnitsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Units controller constructor
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public UnitsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Units
        /// <summary>
        /// Index view. Gets all units.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Units.GetAllAsync();
            return View(res);
        }

        // GET: Units/Details/5
        /// <summary>
        /// Details view. Gets Unit by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Unit object</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _bll.Units.FirstOrDefaultAsync(id.Value);
            
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // GET: Units/Create
        /// <summary>
        /// Create view. Creates viewModel for new Unit.
        /// </summary>
        /// <returns>View with UnitEditViewModel</returns>
        public IActionResult Create()
        {
            var vm = new UnitEditViewModel();
            return View(vm);
        }

        // POST: Units/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create view. Saves new Unit
        /// </summary>
        /// <param name="vm">UnitEditViewModel</param>
        /// <returns>View with UnitEditViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UnitEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Units.Add(vm.Unit);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Units/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets Unit to edit by Id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with UnitEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _bll.Units.FirstOrDefaultAsync(id.Value);
            if (unit == null)
            {
                return NotFound();
            }
            var vm = new UnitEditViewModel();
            vm.Unit = unit;
            return View(vm);
        }

        // POST: Units/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// ModifyAndAdd view. Updates the Unit by Id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">UnitEditViewModel</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UnitEditViewModel vm)
        {
            if (id != vm.Unit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.Units.Update(vm.Unit);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(vm);
        }

        // GET: Units/Delete/5
        /// <summary>
        /// Delte view. Gets Unit to delete by Id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Unit object</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _bll.Units.FirstOrDefaultAsync(id.Value);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // POST: Units/Delete/5
        /// <summary>
        /// Delete view. Deletes the Unit by id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Units.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UnitExists(Guid id)
        {
            return await _bll.Units.ExistsAsync(id);
        }
    }
}
