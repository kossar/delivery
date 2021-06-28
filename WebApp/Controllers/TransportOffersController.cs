using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.TransportOffers;

namespace WebApp.Controllers
{
    /// <summary>
    /// TransportOffers controller. 
    /// </summary>
    [Authorize]
    public class TransportOffersController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// TransportOffers controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public TransportOffersController(IAppBLL bll)
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

            var vm = new TransportOfferDetailsViewModel();
            vm.TransportOffer = transportOffer;
            if (User.Identity?.IsAuthenticated ?? false)
            {
                vm.UserVehiclesCount = await _bll.Vehicles.GetUserVehiclesCount(User.GetUserId()!.Value);
                vm.UserTrailersCount = await _bll.Trailers.GetUserTrailerCount(User.GetUserId()!.Value);
            }
            
            return View(vm);
        }

        // GET: TransportOffers/Create
        /// <summary>
        /// Create view. Gets TransportMeta, Vehicle, Trailer and Organisations.
        /// </summary>
        /// <returns>View with TransportOfferCreateEditViewModel</returns>
        public async Task<IActionResult> Create(Guid? vehicleId, Guid? transportNeedId)
        {
            if (vehicleId == null)
            {
                return NotFound();
            }

            var vehicle = await _bll.Vehicles.FirstOrDefaultAsync(vehicleId.Value, User.GetUserId()!.Value);
            if (vehicle == null)
            {
                return NotFound();
            }

            var vm = new TransportOfferCreateEditViewModel();
            vm.VehicleId = vehicle.Id;
            if (transportNeedId != null)
            {
                vm.TransportNeedId = transportNeedId;
            }
            vm.WeightUnitSelectList =
                new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id), nameof(Unit.UnitCode));
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
        public async Task<IActionResult> Create(TransportOfferCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.TransportOffer.AppUserId = User.GetUserId()!.Value;
                var transportOffer = _bll.TransportOffers.Add(vm.VmToBll());
                await _bll.SaveChangesAsync();
                // If user wants to add Trailer to TransportOffer then Redirect to Trailers
                if (vm.UseTrailer!.Value)
                {
                    return RedirectToAction("TrailerFromSelectListForTransportOffer", "Trailers",
                        new {transportOfferId = transportOffer.Id, transportNeedId = vm.TransportNeedId});
                }
                // If TransportNeedId == null then its not from creating TransportOffer to TransportNeed
                if (vm.TransportNeedId == null)
                {
                    return RedirectToAction(nameof(Details), new {id = transportOffer.Id});
                }
                // If TransportNeedId != null then its  from creating TransportOffer to TransportNeed 
                return RedirectToAction("Create", "Transports",
                    new {transportNeedId = vm.TransportNeedId, transportOfferId = transportOffer.Id});
            }

            vm.WeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id),
                nameof(Unit.UnitCode), vm.TransportOffer.UnitId);
            return View(vm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> VehicleForTransportOffer(string? useprev, Guid? transportNeedId)
        {
            var vm = new TransportOfferVehicleViewModel();
            if (useprev != null)
            {
                vm.UsePreviousVehicle = useprev switch
                {
                    "true" => true,
                    "false" => false,
                    _ => vm.UsePreviousVehicle
                };
            }


            vm.HasPreviusVehicles = await _bll.Vehicles.HasVehicles(User.GetUserId()!.Value);
            // Add default to use previous vehicle - how many vehicles do people use anyway?
            if (vm.HasPreviusVehicles && useprev == null)
            {
                vm.UsePreviousVehicle = true;
            }

            if (transportNeedId != null)
            {
                vm.TransportNeedId = transportNeedId;
            }

            vm.VehicleSelectList = new SelectList(await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value),
                nameof(Vehicle.Id), nameof(Vehicle.Make));
            vm.VehicleVm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(),
                nameof(VehicleType.Id),
                nameof(VehicleType.VehicleTypeName));
            return View(vm);
        }

        /// <summary>
        /// Creates vehicle for Transportoffer
        /// </summary>
        /// <param name="vm">TransportOfferVehicleViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VehicleForTransportOffer(TransportOfferVehicleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.VehicleVm.Vehicle.AppUserId = User.GetUserId()!.Value;
                var vehicle = _bll.Vehicles.Add(vm.VehicleVm.Vehicle);
                await _bll.SaveChangesAsync();
                // if transportneedID == null then this is usual transportoffer adding.
                // If vehicle is saved then redirect to TransportOfferCreate
                if (vm.TransportNeedId == null)
                {
                    return RedirectToAction(nameof(Create), new {vehicleId = vehicle.Id});
                }
                return RedirectToAction(nameof(Create), new {vehicleId = vehicle.Id, transportNeedId = vm.TransportNeedId});
            }

            vm.HasPreviusVehicles = await _bll.Vehicles.HasVehicles(User.GetUserId()!.Value);
            vm.VehicleVm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(),
                nameof(VehicleType.Id),
                nameof(VehicleType.VehicleTypeName), vm.VehicleVm.Vehicle.VehicleTypeId);
            vm.HasPreviusVehicles = await _bll.Vehicles.HasVehicles(User.GetUserId()!.Value);
            vm.VehicleSelectList = new SelectList(await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value),
                nameof(Vehicle.Id), nameof(Vehicle.Make));
            return View(vm);
        }

        /// <summary>
        /// ModifyAndAdd TransportOffer Vehicle - save or select new
        /// </summary>
        /// <param name="id">TransportOffer ID - GUID</param>
        /// <param name="useprev"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditVehicleForTransportOffer(Guid id, string? useprev)
        {
            var transportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(id, User.GetUserId()!.Value);
            if (transportOffer == null)
            {
                return NotFound();
            }

            var vm = new TransportOfferEditVehicleViewModel();
            vm.TransportOfferId = transportOffer.Id;
            if (useprev != null)
            {
                vm.UsePreviousVehicle = useprev switch
                {
                    "true" => true,
                    "false" => false,
                    _ => vm.UsePreviousVehicle
                };
            }

            var userVehicleCount = await _bll.Vehicles.GetUserVehiclesCount(User.GetUserId()!.Value);
            // TODO: If user has one vehicle, then there is no point to display selectList for vehicles
            vm.HasPreviusVehicles = userVehicleCount > 0;
            // Add default to use previous vehicle - how many vehicles do people use anyway?
            if (vm.HasPreviusVehicles && useprev == null && userVehicleCount > 1)
            {
                vm.UsePreviousVehicle = true;
            }

            vm.VehicleSelectList = new SelectList(await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value),
                nameof(Vehicle.Id), nameof(Vehicle.Make));
            vm.VehicleVm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(),
                nameof(VehicleType.Id),
                nameof(VehicleType.VehicleTypeName));
            return View(vm);
        }

        /// <summary>
        /// Save new vehicle and update TransportOffer
        /// </summary>
        /// <param name="vm">TransportOfferEditVehicleViewModel vm</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicleForTransportOffer(TransportOfferEditVehicleViewModel vm)
        {
            var transportOffer =
                await _bll.TransportOffers.FirstOrDefaultAsync(vm.TransportOfferId!.Value, User.GetUserId()!.Value);
            if (transportOffer == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                vm.VehicleVm.Vehicle.AppUserId = User.GetUserId()!.Value;
                await _bll.Vehicles.AddWithTransportOffer(vm.VehicleVm.Vehicle, User.GetUserId()!.Value,
                    vm.TransportOfferId);

                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new {id = transportOffer.Id});
            }

            var userVehicleCount = await _bll.Vehicles.GetUserVehiclesCount(User.GetUserId()!.Value);
            // TODO: If user has one vehicle, then there is no point to display selectList for vehicles
            vm.HasPreviusVehicles = userVehicleCount > 0;

            vm.VehicleSelectList = new SelectList(await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value),
                nameof(Vehicle.Id), nameof(Vehicle.Make));
            vm.VehicleVm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(),
                nameof(VehicleType.Id),
                nameof(VehicleType.VehicleTypeName));
            return View(vm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transportOfferId">transportOfferId GUID</param>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVehicleIdToTransportOffer(Guid transportOfferId, Guid vehicleId)
        {
            var transportOffer =
                await _bll.TransportOffers.FirstOrDefaultAsync(transportOfferId, User.GetUserId()!.Value);
            if (transportOffer == null)
            {
                return NotFound();
            }

            var vehicle = await _bll.Vehicles.FirstOrDefaultAsync(vehicleId, User.GetUserId()!.Value);
            if (vehicle == null)
            {
                return NotFound();
            }

            _bll.TransportOffers.AddVehicleIdToTransportOffer(transportOffer,
                vehicle.Id, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new {id = transportOffer.Id});
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

            var vm = new TransportOfferCreateEditViewModel();
            vm.BllToVm(transportOffer);
            vm.WeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id),
                nameof(Unit.UnitCode), transportOffer.UnitId);
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
        public async Task<IActionResult> Edit(Guid id, TransportOfferCreateEditViewModel vm)
        {
            if (id != vm.TransportOffer.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await TransportOfferExists(vm.TransportOffer.Id))
            {
                vm.WeightUnitSelectList = new SelectList(await _bll.Units.GetWeightUnits(), nameof(Unit.Id),
                    nameof(Unit.UnitCode), vm.TransportOffer.UnitId);
                return View(vm);
            }

            vm.TransportOffer.AppUserId = User.GetUserId()!.Value;
            _bll.TransportOffers.Update(vm.VmToBll());
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new {id = vm.TransportOffer.Id});
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