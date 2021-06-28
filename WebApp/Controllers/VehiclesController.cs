using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Vehicles;

namespace WebApp.Controllers
{
    /// <summary>
    /// Vehicles controller
    /// </summary>
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// VehiclesCrudController constructor
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public VehiclesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Vehicles
        /// <summary>
        /// Index view. Gets All vehicles that have current user id.
        /// </summary>
        /// <returns>View with list of Vehicles</returns>
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Vehicles.GetAllAsync(User.GetUserId()!.Value);
            return View(res);
        }

        // GET: Vehicles/Details/5
        /// <summary>
        /// Details view. Gets Vehicle by id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Vehicle object</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _bll.Vehicles.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }
        

        // GET: Vehicles/Create
        /// <summary>
        /// Create view. Gets VehicleTypes and Organisations.
        /// </summary>
        /// <returns>View with VehicleCreateEditViewModel</returns>
        public  async Task<IActionResult> Create()
        {
            var vm = new VehicleCreateEditViewModel();
            vm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(), nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName));
            return View(vm);
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create view. Saves new Vehicle
        /// </summary>
        /// <param name="vm">VehicleCreateEditViewModel</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.Vehicle.AppUserId = User.GetUserId()!.Value;
                _bll.Vehicles.Add(vm.Vehicle);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(), nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName), vm.Vehicle.VehicleTypeId);
            return View(vm);
        }

        // GET: Vehicles/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets Vehicle to edit by id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with VehicleCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _bll.Vehicles.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (vehicle == null)
            {
                return NotFound();
            }
            var vm = new VehicleCreateEditViewModel();
            vm.Vehicle = vehicle;
            vm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(), nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName), vm.Vehicle.VehicleTypeId);
            return View(vm);
        }

        // POST: Vehicles/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// ModifyAndAdd view. Saves edited Vehicle.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">VehicleCreateEditViewModel</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, VehicleCreateEditViewModel vm)
        {
            if (id != vm.Vehicle.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await VehicleExists(vm.Vehicle.Id))
            {
                vm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(), nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName), vm.Vehicle.VehicleTypeId);
                return View(vm);

            }

            vm.Vehicle.AppUserId = User.GetUserId()!.Value;
            _bll.Vehicles.Update(vm.Vehicle);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        /// <summary>
        /// ModifyAndAdd Vehicle from transportOffer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="offerid">offerid GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> EditFromTransportOffer(Guid? id, Guid? offerid)
        {
            if (id == null || offerid == null)
            {
                return NotFound();
            }

            var vehicle = await _bll.Vehicles.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (vehicle == null)
            {
                return NotFound();
            }
            var vm = new VehicleCreateEditFromTransportOfferViewModel();
            vm.TransportOfferId = offerid!.Value;
            vm.Vehicle = vehicle;
            vm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(), nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName), vm.Vehicle.VehicleTypeId);
            return View(vm);
        }
        
        /// <summary>
        /// Save edited Vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFromTransportOffer(Guid id, VehicleCreateEditFromTransportOfferViewModel vm)
        {
            if (id != vm.Vehicle.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await VehicleExists(vm.Vehicle.Id))
            {
                vm.VehicleTypeSelectList = new SelectList(await _bll.VehicleTypes.GetAllAsync(), nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName), vm.Vehicle.VehicleTypeId);
                return View(vm);

            }

            vm.Vehicle.AppUserId = User.GetUserId()!.Value;
            _bll.Vehicles.Update(vm.Vehicle);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction("Details", "TransportOffers", new {id = vm.TransportOfferId});
        }

        // GET: Vehicles/Delete/5
        /// <summary>
        /// Delete view. Gets Vehicle to delete by Id
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with Vehicle object</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _bll.Vehicles.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        /// <summary>
        /// Delete view. Deletes the Vehicle
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>RedirectToAction(nameof(Index))</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Vehicles.RemoveAsync(id, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> VehicleExists(Guid id)
        {
            return await _bll.Vehicles.ExistsAsync(id, User.GetUserId()!.Value);
        }
    }
}
