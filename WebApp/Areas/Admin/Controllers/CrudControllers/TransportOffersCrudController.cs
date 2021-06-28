using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.TransportOffers;

namespace WebApp.Areas.Admin.Controllers.CrudControllers
{
    /// <summary>
    /// TransportOffers controller. 
    /// </summary>
    [Authorize]
    [Area("Admin")]
    public class TransportOffersCrudController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// TransportOffers controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public TransportOffersCrudController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: TransportOffers
        /// <summary>
        /// Index View. Gets All TransportOffers.
        /// </summary>
        /// <returns>View with List of TransportOffers</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var res = await _bll.TransportOffers.GetAllAsync();
            return View(res);
        }

        // GET: TransportOffers/Details/5
        /// <summary>
        /// Details view. Gets TransportOffer by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportOffer object</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(id.Value);
            if (transportOffer == null)
            {
                return NotFound();
            }

            return View(transportOffer);
        }

        // GET: TransportOffers/Create
        /// <summary>
        /// Create view. Gets TransportMeta, Vehicle, Trailer and Organisations.
        /// </summary>
        /// <returns>View with TransportOfferCreateEditViewModel</returns>
        public async Task<IActionResult> Create()
        {
            var vm = new TransportOfferCreateEditCrudViewModel();
            vm.TransportMetaSelectList = new SelectList(await _bll.TransportMeta.GetAllAsync(User.GetUserId()!.Value), nameof(TransportMeta.Id), nameof(TransportMeta.Id));
            vm.VehicleSelectList = new SelectList(await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value), nameof(Vehicle.Id), nameof(Vehicle.Make));
            vm.TrailerSelectList = new SelectList(await _bll.Trailers.GetAllAsync(User.GetUserId()!.Value), nameof(Trailer.Id), nameof(Trailer.RegNr));
            return View(vm);
        }

        // POST: TransportOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create view. Saves TransportOffer.
        /// </summary>
        /// <param name="vm">TransportOfferCreateEditViewModel</param>
        /// <returns>Redirects to index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransportOfferCreateEditCrudViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.TransportOffer.AppUserId = User.GetUserId()!.Value;
                _bll.TransportOffers.Add(vm.TransportOffer);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.TransportMetaSelectList = new SelectList(await _bll.TransportMeta.GetAllAsync(User.GetUserId()!.Value), nameof(TransportMeta.Id), nameof(TransportMeta.Id), vm.TransportOffer.TransportMeta);
            vm.VehicleSelectList = new SelectList(await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value), nameof(Vehicle.Id), nameof(Vehicle.Make), vm.TransportOffer.VehicleId);
            vm.TrailerSelectList = new SelectList(await _bll.Trailers.GetAllAsync(User.GetUserId()!.Value), nameof(Trailer.Id), nameof(Trailer.RegNr), vm.TransportOffer.TrailerId);
            return View(vm);
        }

        // GET: TransportOffers/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets TransportOffer to edit by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportOfferCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (transportOffer == null)
            {
                return NotFound();
            }
            var vm = new TransportOfferCreateEditCrudViewModel();
            vm.TransportOffer = transportOffer;
            vm.TransportMetaSelectList = new SelectList(await _bll.TransportMeta.GetAllAsync(User.GetUserId()!.Value), nameof(TransportMeta.Id), nameof(TransportMeta.Id), vm.TransportOffer.TransportMeta);
            vm.VehicleSelectList = new SelectList(await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value), nameof(Vehicle.Id), nameof(Vehicle.Make), vm.TransportOffer.VehicleId);
            vm.TrailerSelectList = new SelectList(await _bll.Trailers.GetAllAsync(User.GetUserId()!.Value), nameof(Trailer.Id), nameof(Trailer.RegNr), vm.TransportOffer.TrailerId);
            return View(vm);
        }

        // POST: TransportOffers/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// ModifyAndAdd view. Saves edited TransportOffer object.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">TransportOfferCreateEditViewModel</param>
        /// <returns>Redirects to Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TransportOfferCreateEditCrudViewModel vm)
        {
            if (id != vm.TransportOffer.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await TransportOfferExists(vm.TransportOffer.Id))
            {
                vm.TransportMetaSelectList = new SelectList(await _bll.TransportMeta.GetAllAsync(User.GetUserId()!.Value), nameof(TransportMeta.Id), nameof(TransportMeta.Id), vm.TransportOffer.TransportMeta);
                vm.VehicleSelectList = new SelectList(await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value), nameof(Vehicle.Id), nameof(Vehicle.Make), vm.TransportOffer.VehicleId);
                vm.TrailerSelectList = new SelectList(await _bll.Trailers.GetAllAsync(User.GetUserId()!.Value), nameof(Trailer.Id), nameof(Trailer.RegNr), vm.TransportOffer.TrailerId);
                return View(vm);
            }

            vm.TransportOffer.AppUserId = User.GetUserId()!.Value;
            _bll.TransportOffers.Update(vm.TransportOffer);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        // GET: TransportOffers/Delete/5
        /// <summary>
        /// Delete view. Gets TransportOffer to delete by Id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportOfferObject</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (transportOffer == null)
            {
                return NotFound();
            }

            return View(transportOffer);
        }

        // POST: TransportOffers/Delete/5
        /// <summary>
        /// Delete view. Deletes the TransportOffer by Id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>Redirects to Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.TransportOffers.RemoveAsync(id, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TransportOfferExists(Guid id)
        {
            return await _bll.TransportOffers.ExistsAsync(id, User.GetUserId()!.Value);
        }
    }
}
