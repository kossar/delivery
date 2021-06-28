using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Trailers;

namespace WebApp.Areas.Admin.Controllers.CrudControllers
{
    /// <summary>
    /// Trailers Controller
    /// </summary>
    [Authorize]
    [Area("Admin")]
    public class TrailersCrudController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Trailers controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public TrailersCrudController( IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Trailers
        /// <summary>
        /// Index view. Gets All trailers.
        /// </summary>
        /// <returns>View with List of Trailer objects.</returns>
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Trailers.GetAllAsync(User.GetUserId()!.Value);
            return View(res);
        }

        // GET: Trailers/Details/5
        /// <summary>
        /// Details view. Gets one Trailer by Id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Trailer object.</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trailer = await _bll.Trailers.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (trailer == null)
            {
                return NotFound();
            }

            return View(trailer);
        }

        // GET: Trailers/Create
        /// <summary>
        /// Create view. Gets dimensions, units
        /// </summary>
        /// <returns>View with TrailerCreateEditViewModel</returns>
        public async Task<IActionResult> Create()
        {
            var vm = new TrailerCreateEditCrudViewModel();
            vm.DimensionsSelectList = new SelectList(await _bll.Dimensions.GetAllAsync(User.GetUserId()!.Value), nameof(Dimensions.Id), nameof(Dimensions.Id));
            vm.UnitsSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode));
            return View(vm);
        }

        // POST: Trailers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create view. Saves new Trailer object.
        /// </summary>
        /// <param name="vm">TrailerCreateEditViewModel</param>
        /// <returns>View with TrailerCreateEditViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrailerCreateEditCrudViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("Trailer create modelstate valid");
                vm.Trailer.AppUserId = User.GetUserId()!.Value;
                _bll.Trailers.Add(vm.Trailer);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine("Trailer create modelstate NOT valid");
            vm.DimensionsSelectList = new SelectList(await _bll.Dimensions.GetAllAsync(), nameof(Dimensions.Id), nameof(Dimensions.Id), vm.Trailer.DimensionsId);
            vm.UnitsSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Trailer.UnitId);
            return View(vm);
        }

        // GET: Trailers/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets Trailer by Id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TrailerCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trailer = await _bll.Trailers.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (trailer == null)
            {
                return NotFound();
            }

            var vm = new TrailerCreateEditCrudViewModel();
            vm.Trailer = trailer;
            
            vm.DimensionsSelectList = new SelectList(await _bll.Dimensions.GetAllAsync(), nameof(Dimensions.Id), nameof(Dimensions.Id), vm.Trailer.DimensionsId);
            vm.UnitsSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Trailer.UnitId);
            return View(vm);
        }

        // POST: Trailers/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// ModifyAndAdd view. Updates Trailer.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">TrailerCreateEditViewModel</param>
        /// <returns>Redirects to index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TrailerCreateEditCrudViewModel vm)
        {
            if (id != vm.Trailer.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await _bll.Trailers.ExistsAsync(id, User.GetUserId()!.Value))
            {
                vm.DimensionsSelectList = new SelectList(await _bll.Dimensions.GetAllAsync(), nameof(Dimensions.Id), nameof(Dimensions.Id), vm.Trailer.DimensionsId);
                vm.UnitsSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Trailer.UnitId);
                return View(vm);

            }
            _bll.Trailers.Update(vm.Trailer);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Trailers/Delete/5
        /// <summary>
        /// Delete view. Gets Trailer to delete by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with trailer object</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trailer = await _bll.Trailers.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (trailer == null)
            {
                return NotFound();
            }

            return View(trailer);
        }

        // POST: Trailers/Delete/5
        /// <summary>
        /// Delete view. Deletes Trailer by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>Redirects to index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Trailers.RemoveAsync(id, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TrailerExists(Guid id)
        {
            return await _bll.Trailers.ExistsAsync(id, User.GetUserId()!.Value);
        }
    }
}
