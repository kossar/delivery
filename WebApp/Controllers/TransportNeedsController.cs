using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using BLL.App.DTO;
using BLL.App.DTO.Enums;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ApiControllers;
using WebApp.ViewModels.TransportNeeds;

namespace WebApp.Controllers
{
    /// <summary>
    /// TransportNeed Controller 
    /// </summary>
    [Authorize]
    public class TransportNeedsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// TransportNeed controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public TransportNeedsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: TransportNeeds
        /// <summary>
        /// Index view. Gets TransportNeeds.
        /// </summary>
        /// <returns>view with List of TransportNeed objects</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var res = await _bll.TransportNeeds.GetByCountAsync(5);
            return View(res);
        }

        // GET: TransportNeeds/Details/5
        /// <summary>
        /// Details view. Gets TransportNeed by id.
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>View TransportNeed object</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(id.Value);

            if (transportNeed == null)
            {
                return NotFound();
            }

            return View(transportNeed);
        }

        // GET: TransportNeeds/Create
        /// <summary>
        /// Create view. 
        /// </summary>
        /// <returns>View with TransportNeedCreateEditViewModel</returns>
        public IActionResult Create(Guid? transportOfferId)
        {
            var vm = new TransportNeedCreateEditViewModel();
            if (transportOfferId != null)
            {
                vm.TransportOfferId = transportOfferId;
            }

            return View(vm);
        }

        // POST: TransportNeeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create view. Saves TransportNeed.
        /// </summary>
        /// <param name="vm">TransportNeedCreateEditViewModel</param>
        /// <returns>Redirects to Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransportNeedCreateEditViewModel vm)
        {
           
            
            if (ModelState.IsValid)
            {
                vm.TransportMeta.StartLocationId = _bll.Locations.Add(vm.StartLocation).Id;
                vm.TransportMeta.DestinationLocationId = _bll.Locations.Add(vm.DestinationLocation).Id;
                vm.TransportNeed.TransportMetaId = _bll.TransportMeta.Add(vm.TransportMeta).Id;
                vm.TransportNeed.AppUserId = User.GetUserId()!.Value;
                var savedTransportNeed = _bll.TransportNeeds.Add(vm.TransportNeed);
                await _bll.SaveChangesAsync();
                if (vm.TransportNeed.TransportType != ETransportType.PersonsOnly)
                {
                    return RedirectToAction(nameof(Create), "Parcels", 
                        new {transportNeedId = savedTransportNeed.Id, transportOfferId = vm.TransportOfferId});
                }

                if (vm.TransportOfferId == null)
                {
                    return RedirectToAction(nameof(Details), new {id = savedTransportNeed.Id});
                }
                return RedirectToAction(nameof(Create), "Transports",
                    new {transportNeedId = savedTransportNeed.Id, transportOfferId = vm.TransportOfferId});

            }

            return View(vm);
        }


        // GET: TransportNeeds/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets TransportNeed to edit by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportNeedCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (transportNeed == null)
            {
                return NotFound();
            }

            var vm = new TransportNeedCreateEditViewModel();
            vm.MapToVm(transportNeed);
            return View(vm);
        }

        // POST: TransportNeeds/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// ModifyAndAdd view. 
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">TransportNeedCreateEditViewModel</param>
        /// <returns>Redirects to index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TransportNeedCreateEditViewModel vm)
        {
            if (id != vm.TransportNeed.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || !await _bll.TransportNeeds.ExistsAsync(id, User.GetUserId()!.Value))
            {
                return View(vm);
            }

            vm.TransportNeed.AppUserId = User.GetUserId()!.Value;
            _bll.TransportNeeds.Update(vm.VmToBll());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TransportNeeds/Delete/5
        /// <summary>
        /// Delete view. Gets TransportNeed to delete by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportNeed object</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);

            if (transportNeed == null)
            {
                return NotFound();
            }

            return View(transportNeed);
        }

        // POST: TransportNeeds/Delete/5
        /// <summary>
        /// Delete view. Deletes Transportneed by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.TransportNeeds.RemoveAsync(id, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TransportNeedExists(Guid id)
        {
            return await _bll.TransportNeeds.ExistsAsync(id, User.GetUserId()!.Value);
        }
    }
}