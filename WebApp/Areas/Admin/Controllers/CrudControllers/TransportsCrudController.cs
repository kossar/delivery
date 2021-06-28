using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Transports;

namespace WebApp.Areas.Admin.Controllers.CrudControllers
{
    /// <summary>
    /// Transports controller.
    /// </summary>
    [Authorize]
    [Area("Admin")]
    public class TransportsCrudController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Transports controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public TransportsCrudController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Transports
        /// <summary>
        /// Index view. Gets all Transports.
        /// </summary>
        /// <returns>View with a List of Transports</returns>
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Transports.GetAllAsync();
            return View(res);
        }

        // GET: Transports/Details/5
        /// <summary>
        /// Details view. Gets Transport object by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Transport object</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _bll.Transports.FirstOrDefaultAsync(id.Value);
            if (transport == null)
            {
                return NotFound();
            }

            return View(transport);
        }

        // GET: Transports/Create
        /// <summary>
        /// Create view. Gets Locations, TransportNeeds and TransportOffers
        /// </summary>
        /// <returns>View with TransportCreateEditViewModel</returns>
        public async Task<IActionResult> Create()
        {
            var vm = new TransportCreateEditCrudViewModel();
            vm.PickUpLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(), nameof(Location.Id), nameof(Location.City));
            vm.TransportNeedSelectList = new SelectList(await _bll.TransportNeeds.GetAllAsync(), nameof(TransportNeed.Id), nameof(TransportNeed.Id));
            vm.TransportOfferSelectList = new SelectList(await _bll.TransportOffers.GetAllAsync(), nameof(TransportOffer.Id), nameof(TransportOffer.Id));
            return View(vm);
        }

        // POST: Transports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create view. Saves Transport.
        /// </summary>
        /// <param name="vm">TransportCreateEditViewModel</param>
        /// <returns>View with TransportCreateEditViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransportCreateEditCrudViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Transports.Add(vm.Transport);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.PickUpLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(), nameof(Location.Id), nameof(Location.City), vm.Transport.PickUpLocationId);
            vm.TransportNeedSelectList = new SelectList(await _bll.TransportNeeds.GetAllAsync(), nameof(TransportNeed.Id), nameof(TransportNeed.Id), vm.Transport.TransportNeedId);
            vm.TransportOfferSelectList = new SelectList(await _bll.TransportOffers.GetAllAsync(), nameof(TransportOffer.Id), nameof(TransportOffer.Id), vm.Transport.TransportOfferId);
            return View(vm);
        }

        // GET: Transports/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets Transport to edit by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _bll.Transports.FirstOrDefaultAsync(id.Value);
            if (transport == null)
            {
                return NotFound();
            }
            var vm = new TransportCreateEditCrudViewModel();
            vm.Transport = transport;
            vm.PickUpLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(), nameof(Location.Id), nameof(Location.City), vm.Transport.PickUpLocationId);
            vm.TransportNeedSelectList = new SelectList(await _bll.TransportNeeds.GetAllAsync(), nameof(TransportNeed.Id), nameof(TransportNeed.Id), vm.Transport.TransportNeedId);
            vm.TransportOfferSelectList = new SelectList(await _bll.TransportOffers.GetAllAsync(), nameof(TransportOffer.Id), nameof(TransportOffer.Id), vm.Transport.TransportOfferId);
            return View(vm);
        }

        // POST: Transports/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// ModifyAndAdd view. Updates the Transport object.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">TransportCreateEditViewModel</param>
        /// <returns>Redirects to Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TransportCreateEditCrudViewModel vm)
        {
            if (id != vm.Transport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.Transports.Update(vm.Transport);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            vm.PickUpLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(), nameof(Location.Id), nameof(Location.City), vm.Transport.PickUpLocationId);
            vm.TransportNeedSelectList = new SelectList(await _bll.TransportNeeds.GetAllAsync(), nameof(TransportNeed.Id), nameof(TransportNeed.Id), vm.Transport.TransportNeedId);
            vm.TransportOfferSelectList = new SelectList(await _bll.TransportOffers.GetAllAsync(), nameof(TransportOffer.Id), nameof(TransportOffer.Id), vm.Transport.TransportOfferId);
            return View(vm);
        }

        // GET: Transports/Delete/5
        /// <summary>
        /// Delete view. Gets Transport to delete by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Transport object</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _bll.Transports.FirstOrDefaultAsync(id.Value);
            if (transport == null)
            {
                return NotFound();
            }

            return View(transport);
        }

        // POST: Transports/Delete/5
        /// <summary>
        /// Delete view. Deletes Transport by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Transports.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TransportExists(Guid id)
        {
            return await _bll.Transports.ExistsAsync(id);
        }
    }
}
