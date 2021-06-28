using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Parcels;

namespace WebApp.Areas.Admin.Controllers.CrudControllers
{
    /// <summary>
    /// Controller for Parcels
    /// </summary>
    [Authorize]
    [Area("Admin")]
    public class ParcelsCrudController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// ParcelController constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public ParcelsCrudController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Parcels
        /// <summary>
        /// Index view. Gets all Parcels.
        /// </summary>
        /// <returns>View with list of Parcel objects.</returns>
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Parcels.GetAllAsync(User.GetUserId()!.Value);
            return View(res);
        }

        // GET: Parcels/Details/5
        /// <summary>
        /// Parcel details view. Gets one parcel.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Parcel object.</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _bll.Parcels.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (parcel == null)
            {
                return NotFound();
            }

            return View(parcel);
        }

        // GET: Parcels/Create
        /// <summary>
        /// Create view. Gets dimensions, transportneeds and units.
        /// </summary>
        /// <returns>View with ParcelCreateEditViewModel</returns>
        public async Task<IActionResult> Create()
        {
            var vm = new ParcelCreateEditCrudViewModel();
            vm.DimensionsSelectList = new SelectList(await _bll.Dimensions.GetAllAsync(), nameof(Dimensions.Id), nameof(Dimensions.Id));
            vm.TransportNeedsSelectList = new SelectList(await _bll.TransportNeeds.GetAllAsync(), nameof(TransportNeed.Id), nameof(TransportNeed.Id));
            vm.UnitsSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode));
            return View(vm);
        }

        // POST: Parcels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create view for Parcel. Saves Parcel object.
        /// </summary>
        /// <param name="vm">ParcelCreateEditViewModel</param>
        /// <returns>View with ParcelCreateEditViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParcelCreateEditCrudViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Parcels.Add(vm.Parcel);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.DimensionsSelectList = new SelectList(await _bll.Dimensions.GetAllAsync(), nameof(Dimensions.Id), nameof(Dimensions.Id), vm.Parcel.DimensionsId);
            vm.TransportNeedsSelectList = new SelectList(await _bll.TransportNeeds.GetAllAsync(), nameof(TransportNeed.Id), nameof(TransportNeed.Id), vm.Parcel.TransportNeedId);
            vm.UnitsSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Parcel.UnitId);
            return View(vm);
        }

        // GET: Parcels/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets Parcel to edit by ID.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with ParcelCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _bll.Parcels.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (parcel == null)
            {
                return NotFound();
            }

            var vm = new ParcelCreateEditCrudViewModel();
            vm.Parcel = parcel;
            
            vm.DimensionsSelectList = new SelectList(await _bll.Dimensions.GetAllAsync(), nameof(Dimensions.Id), nameof(Dimensions.Id), vm.Parcel.DimensionsId);
            vm.TransportNeedsSelectList = new SelectList(await _bll.TransportNeeds.GetAllAsync(), nameof(TransportNeed.Id), nameof(TransportNeed.Id), vm.Parcel.TransportNeedId);
            vm.UnitsSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Parcel.UnitId);
            return View(vm);
        }

        // POST: Parcels/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// ModifyAndAdd view. Updates Selected Parcel.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">ParcelCreateEditViewModel</param>
        /// <returns>Redirects to Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ParcelCreateEditCrudViewModel vm)
        {
            if (id != vm.Parcel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await _bll.Parcels.ExistsAsync(vm.Parcel.Id))
            {
                vm.DimensionsSelectList = new SelectList(await _bll.Dimensions.GetAllAsync(), nameof(Dimensions.Id), nameof(Dimensions.Id), vm.Parcel.DimensionsId);
                vm.TransportNeedsSelectList = new SelectList(await _bll.TransportNeeds.GetAllAsync(), nameof(TransportNeed.Id), nameof(TransportNeed.Id), vm.Parcel.TransportNeedId);
                vm.UnitsSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Parcel.UnitId);
                return View(vm);

                
            }
            _bll.Parcels.Update(vm.Parcel);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Parcels/Delete/5
        /// <summary>
        /// Delete view. Gets Parcel to delete by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Parcel object</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parcel = await _bll.Parcels.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (parcel == null)
            {
                return NotFound();
            }

            return View(parcel);
        }

        // POST: Parcels/Delete/5
        /// <summary>
        /// Delete view. Deletes Parcel by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>Redirects to Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Parcels.RemoveAsync(id, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
