using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Parcels;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for Parcels
    /// </summary>
    [Authorize]
    public class ParcelsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// ParcelController constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public ParcelsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Parcels/Create
        /// <summary>
        /// Create view. Gets dimensions, transportneeds and units.
        /// </summary>
        /// <returns>View with ParcelCreateEditViewModel</returns>
        public async Task<IActionResult> Create(Guid? transportNeedId, Guid? transportOfferId)
        {
            if (transportNeedId == null)
            {
                return NotFound();
            }

            var transportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(transportNeedId.Value, User.GetUserId()!.Value);
            if (transportNeed == null)
            {
                return NotFound();
            }
            var vm = new ParcelCreateEditViewModel();
            vm.TransportNeedId = transportNeed.Id;
            if (transportOfferId != null)
            {
                vm.TransportOfferId = transportOfferId;
            }
            vm.ParcelWeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id), nameof(Unit.UnitCode));
            vm.DimensionUnitSelectList = new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id), nameof(Unit.UnitCode));
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
        public async Task<IActionResult> Create(ParcelCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.Parcel.TransportNeedId = vm.TransportNeedId;
                _bll.Parcels.Add(vm.Parcel);
                await _bll.SaveChangesAsync();
                if (vm.TransportOfferId == null)
                {
                    return RedirectToAction("Details", "TransportNeeds", new {id = vm.Parcel.TransportNeedId});
                }
                return RedirectToAction("Create", "Transports", new {transportNeedId = vm.Parcel.TransportNeedId, transportOfferId = vm.TransportOfferId});
            }
            vm.ParcelWeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Parcel.UnitId);
            vm.DimensionUnitSelectList = new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Parcel.Dimensions!.UnitId);
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

            var vm = new ParcelCreateEditViewModel();
            vm.Parcel = parcel;
            
            vm.ParcelWeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Parcel.UnitId);
            vm.DimensionUnitSelectList = new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Parcel.Dimensions!.UnitId);
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
        public async Task<IActionResult> Edit(Guid id, ParcelCreateEditViewModel vm)
        {
            if (id != vm.Parcel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await _bll.Parcels.ExistsAsync(vm.Parcel.Id))
            {
                vm.ParcelWeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Parcel.UnitId);
                vm.DimensionUnitSelectList = new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id), nameof(Unit.UnitCode), vm.Parcel.Dimensions!.UnitId);
                return View(vm);

                
            }
            _bll.Parcels.Update(vm.Parcel);
            await _bll.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }

    }
}
