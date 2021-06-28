using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Trailers;

namespace WebApp.Controllers
{
    /// <summary>
    /// Trailers Controller
    /// </summary>
    [Authorize]
    public class TrailersController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Trailers controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public TrailersController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Trailers
        /// <summary>
        /// Index view. Gets All User trailers.
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

        /// <summary>
        /// Trailer for transport offer action
        /// </summary>
        /// <param name="transportOfferId">TransportOffer id, GUID</param>
        /// <param name="transportNeedId"></param>
        /// <returns></returns>
        public async Task<IActionResult> NewTrailerForTransportOffer(Guid transportOfferId, Guid? transportNeedId)
        {
            var vm = new TrailerCreateEditViewModel();
            vm.UserHasTrailers = await _bll.Trailers.UserHasTrailers(User.GetUserId()!.Value);
            vm.TransportOfferId = transportOfferId;
            if (transportNeedId != null)
            {
                vm.TransportNeedId = transportNeedId;
            }
            
            vm.TrailerLoadWeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id),
                nameof(Unit.UnitCode));
            vm.TrailerSizeSelectList =
                new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id), nameof(Unit.UnitCode));
            return View(vm);
        }

        /// <summary>
        /// Save new trailer, and adds trailerId to TransportOffer
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewTrailerForTransportOffer(TrailerCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.Trailer.AppUserId = User.GetUserId()!.Value;
                await _bll.Trailers.AddWithTransportOffer(vm.Trailer, User.GetUserId()!.Value, vm.TransportOfferId);
                
                await _bll.SaveChangesAsync();
                if (vm.TransportNeedId != null)
                {
                    return RedirectToAction("Create", "Transports",
                        new {transportNeedId = vm.TransportNeedId, transportOfferId = vm.TransportOfferId});
                }
                return RedirectToAction("Details", "TransportOffers", new {id = vm.TransportOfferId});
            }

            vm.UserHasTrailers = await _bll.Trailers.UserHasTrailers(User.GetUserId()!.Value);
            vm.TrailerLoadWeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id),
                nameof(Unit.UnitCode));
            vm.TrailerSizeSelectList =
                new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id), nameof(Unit.UnitCode));
            return View(vm);
        }

        /// <summary>
        /// Select Trailer for transportOffer from selectList
        /// </summary>
        /// <param name="transportOfferId"></param>
        /// <param name="transportNeedId"></param>
        /// <returns></returns>
        public async Task<IActionResult> TrailerFromSelectListForTransportOffer(Guid transportOfferId, Guid? transportNeedId)
        {
            var transportOffer =
                await _bll.TransportOffers.FirstOrDefaultAsync(transportOfferId, User.GetUserId()!.Value);
            if (transportOffer == null)
            {
                return NotFound();
            }

            var userHasTrailers = await _bll.Trailers.UserHasTrailers(User.GetUserId()!.Value);

            // If User doesnt have Trailers, then redirect to add new trailer.
            if (!userHasTrailers)
            {
                return RedirectToAction(nameof(NewTrailerForTransportOffer), new {transportOfferId = transportOfferId, transportNeedId = transportNeedId});
            }

            var vm = new TrailerFromSelectListViewModel();
            vm.TransportOfferId = transportOfferId;
            if (transportNeedId != null)
            {
                vm.TransportNeedId = transportNeedId;
            }
            vm.TrailerSelectList = new SelectList(await _bll.Trailers.GetAllAsync(User.GetUserId()!.Value),
                nameof(Trailer.Id), nameof(Trailer.RegNr));
            return View(vm);
        }

        /// <summary>
        /// Adds TrailerId to TransportOffer
        /// </summary>
        /// <param name="vm">TrailerFromSelectListViewModel vm</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TrailerFromSelectListForTransportOffer(TrailerFromSelectListViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await _bll.TransportOffers.AddTrailerIdToTransportOffer(vm.TransportOfferId!.Value,
                    vm.TrailerId!.Value);
                await _bll.SaveChangesAsync();
                if (vm.TransportNeedId != null)
                {
                    return RedirectToAction("Create", "Transports",
                        new {transportNeedId = vm.TransportNeedId, transportOfferId = vm.TransportOfferId});
                }
                return RedirectToAction("Details", "TransportOffers", new {id = vm.TransportOfferId});
            }

            vm.TrailerSelectList = new SelectList(await _bll.Trailers.GetAllAsync(User.GetUserId()!.Value),
                nameof(Trailer.Id), nameof(Trailer.RegNr));
            return View(vm);
        }

        /// <summary>
        /// Action to remove Trailer from TransportOffer
        /// </summary>
        /// <param name="id">GUID id - Trailer Id</param>
        /// <param name="transportOfferId">GUID transportOffer id</param>
        /// <returns>View with TrailerWithTransportOfferIdViewModel</returns>
        public async Task<IActionResult> RemoveTrailerFromTransportOffer(Guid? id, Guid? transportOfferId)
        {
            if (id == null || transportOfferId == null)
            {
                return NotFound();
            }

            var trailer = await _bll.Trailers.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (trailer == null)
            {
                return NotFound();
            }

            var vm = new TrailerWithTransportOfferIdViewModel
            {
                Trailer = trailer, TransportOfferId = transportOfferId.Value
            };

            return View(vm);
            // var transportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(transportOfferId.Value, User.GetUserId()!.Value);
            // if (transportOffer == null)
            // {
            //     return NotFound();
            // }

        }
        
        /// <summary>
        /// Removes Trailer from transportOffer
        /// </summary>
        /// <param name="vm">TrailerWithTransportOfferIdViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTrailerFromTransportOffer(TrailerWithTransportOfferIdViewModel vm)
        {
            var transportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(vm.TransportOfferId, User.GetUserId()!.Value);
            if (transportOffer == null)
            {
                return NotFound();
            }

            var newTransportOffer = _bll.TransportOffers.RemoveTrailerFromTransportOffer(transportOffer);
            await _bll.SaveChangesAsync();

            return RedirectToAction("Details", "TransportOffers", new {id = newTransportOffer!.Id});
        }

        // GET: Trailers/Create
        /// <summary>
        /// Create view. Gets dimensions, units
        /// </summary>
        /// <returns>View with TrailerCreateEditViewModel</returns>
        public async Task<IActionResult> Create()
        {
            var vm = new TrailerCreateEditViewModel();
            vm.TrailerLoadWeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id),
                nameof(Unit.UnitCode));
            vm.TrailerSizeSelectList =
                new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id), nameof(Unit.UnitCode));
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
        public async Task<IActionResult> Create(TrailerCreateEditViewModel vm)
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
            vm.TrailerSizeSelectList = new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id),
                nameof(Unit.UnitCode), vm.Trailer.Dimensions!.UnitId);
            vm.TrailerLoadWeightUnitSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id),
                nameof(Unit.UnitCode), vm.Trailer.UnitId);
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

            var vm = new TrailerCreateEditViewModel();
            vm.Trailer = trailer;

            vm.TrailerSizeSelectList = new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id),
                nameof(Unit.UnitCode), vm.Trailer.Dimensions!.UnitId);
            vm.TrailerLoadWeightUnitSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id),
                nameof(Unit.UnitCode), vm.Trailer.UnitId);
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
        public async Task<IActionResult> Edit(Guid id, TrailerCreateEditViewModel vm)
        {
            if (id != vm.Trailer.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await _bll.Trailers.ExistsAsync(id, User.GetUserId()!.Value))
            {
                vm.TrailerSizeSelectList = new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id),
                    nameof(Unit.UnitCode), vm.Trailer.Dimensions!.UnitId);
                vm.TrailerLoadWeightUnitSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id),
                    nameof(Unit.UnitCode), vm.Trailer.UnitId);
                return View(vm);
            }

            vm.Trailer.AppUserId = User.GetUserId()!.Value;
            _bll.Trailers.Update(vm.Trailer);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// ModifyAndAdd trailer after coming from transportoffer details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="offerid"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditFromTransportOffer(Guid? id, Guid? offerid)
        {
            if (id == null || offerid == null)
            {
                return NotFound();
            }

            var trailer = await _bll.Trailers.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (trailer == null)
            {
                return NotFound();
            }

            var vm = new TrailerCreateEditViewModel();
            vm.Trailer = trailer;
            vm.TransportOfferId = offerid;

            vm.TrailerSizeSelectList = new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id),
                nameof(Unit.UnitCode), vm.Trailer.Dimensions!.UnitId);
            vm.TrailerLoadWeightUnitSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id),
                nameof(Unit.UnitCode), vm.Trailer.UnitId);
            return View(vm);
        }

        /// <summary>
        /// Update trailer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFromTransportOffer(Guid id, TrailerCreateEditViewModel vm)
        {
            if (id != vm.Trailer.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await _bll.Trailers.ExistsAsync(id, User.GetUserId()!.Value))
            {
                vm.TrailerSizeSelectList = new SelectList(await _bll.Units.GetLengthUnits(), nameof(Unit.Id),
                    nameof(Unit.UnitCode), vm.Trailer.Dimensions!.UnitId);
                vm.TrailerLoadWeightUnitSelectList = new SelectList(await _bll.Units.GetAllAsync(), nameof(Unit.Id),
                    nameof(Unit.UnitCode), vm.Trailer.UnitId);
                return View(vm);
            }

            vm.Trailer.AppUserId = User.GetUserId()!.Value;
            _bll.Trailers.Update(vm.Trailer);
            await _bll.SaveChangesAsync();
            return RedirectToAction("Details", "TransportOffers", new {id = vm.TransportOfferId});
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