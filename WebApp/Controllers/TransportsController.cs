using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.DTO.Enums;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.Transports;

namespace WebApp.Controllers
{
    /// <summary>
    /// Transports controller.
    /// </summary>
    [Authorize]
    public class TransportsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Transports controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public TransportsController(IAppBLL bll)
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
            var vm = new TransportIndexViewModel();
            vm.UserPendingTransportOfferTransports = 
                await _bll.Transports.GetUserTransportOfferTransports(User.GetUserId()!.Value);
            vm.UserPendingTransportNeedTransports = 
                await _bll.Transports.GetUserTransportNeedTransports(User.GetUserId()!.Value);
            vm.UserTransportNeedsWaitingForUser =
                await _bll.Transports.GetUserTransportNeedsWaitingForUserAction(User.GetUserId()!.Value);
            vm.UserTransportOffersWaitingForUser =
                await _bll.Transports.GetUserTransportOffersWaitingForUserAction(User.GetUserId()!.Value);
            return View(vm);
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

            var transport = await _bll.Transports.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (transport == null)
            {
                return NotFound();
            }

            transport.TransportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(transport.TransportNeedId);
            transport.TransportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(transport.TransportOfferId);

            return View(transport);
        }
        
        
        /// <summary>
        /// Create TransportOffer to TransportNeed
        /// </summary>
        /// <param name="id">TransportNeedID - GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> StartFromTransportNeed(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // TransportNeed for what user adds an offer
            var transportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(id.Value);
            
            if (transportNeed == null)
            {
                return NotFound();
            }
            
            // User TransportOffers to take offer for transportneed
            var offers = await _bll.TransportOffers.GetUserUnFinishedTransportOffers(User.GetUserId()!.Value);
            var transportOffers = offers.ToList();
            
            if (!transportOffers.Any())
            {
                //Redirect to add new offer
                return RedirectToAction("VehicleForTransportOffer", "TransportOffers",
                    new {transportNeedId = transportNeed.Id});
            }

            var vm = new TransportCreateEditFromNeedViewModel();
            var selectList =  transportOffers.Select(x => new SelectListItem()
            {
                Text = x.TransportMeta!.StartTime + " " + x!.TransportMeta!.StartLocation!.City,
                Value = x.Id.ToString(),
            }).ToList();
            vm.TransportNeedId = id.Value;
            vm.TransportOfferSelectList = new SelectList(selectList,"Value","Text");
            return View(vm);
        }
        
        /// <summary>
        /// Create TransportOffer to TransportNeed
        /// </summary>
        /// <param name="id">TransportNeedID - GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> StartFromTransportOffer(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // TransportOffer for what user adds an offer
            var transportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(id.Value);
            
            if (transportOffer == null)
            {
                return NotFound();
            }
            
            // User transportNeeds to take offer for transportOffer
            var needs = await _bll.TransportNeeds.GetUserUnFinishedTransportNeeds(User.GetUserId()!.Value);
            var transportNeeds = needs.ToList();
            
            if (!transportNeeds.Any())
            {
                //Redirect to add new offer
                return RedirectToAction("Create", "TransportNeeds",
                    new {transportOfferId = transportOffer.Id});
            }

            var vm = new TransportCreateEditFromOfferViewModel();
            var selectList =  transportNeeds.Select(x => new SelectListItem()
            {
                Text = x.TransportMeta!.StartTime + " " + x!.TransportMeta!.StartLocation!.City,
                Value = x.Id.ToString(),
            }).ToList();
            vm.TransportNeedId = id.Value;
            vm.TransportOfferId = transportOffer.Id;
            vm.TransportNeedSelectList = new SelectList(selectList,"Value","Text");
            return View(vm);
        }
        
        
        /// <summary>
        /// Make transport - Add transportOffer and transportNeed to Transport
        /// </summary>
        /// <param name="transportNeedId">transportNeedId GUID</param>
        /// <param name="transportOfferId">transportOfferId GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> Create(Guid? transportNeedId, Guid? transportOfferId)
        {
            if (transportNeedId == null || transportOfferId == null)
            {
                return NotFound();
            }
            
            var transportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(transportNeedId.Value);
            
            if (transportNeed == null)
            {
                return NotFound();
            }

            var transportOffer =
                await _bll.TransportOffers.FirstOrDefaultAsync(transportOfferId.Value);
            if (transportOffer == null)
            {
                return NotFound();
            }


            var vm = new TransportCreateEditViewModel
            {
                TransportNeedId = transportNeedId.Value, 
                TransportOfferId = transportOfferId.Value,
                TransportOffer = transportOffer,
                TransportNeed = transportNeed,
                Transport = new Transport()
                {
                    PickUpTime = transportNeed.TransportMeta!.StartTime, 
                    PickUpLocation = transportNeed.TransportMeta!.StartLocation
                }
            };
            return View(vm);
        }
        
        /// <summary>
        /// Save Transport
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransportCreateEditViewModel vm)
        {
            
            if (ModelState.IsValid)
            {
                await _bll.Transports.InitialTransportAdd(vm.Transport, vm.TransportOfferId, vm.TransportNeedId, User.GetUserId()!.Value);
                await _bll.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            vm.TransportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(vm.TransportNeedId);
            vm.TransportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(vm.TransportOfferId);
            return View(vm);
        }
        
        /// <summary>
        /// Set new status to transport
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IActionResult> Actions(Guid? id, ETransportStatus? status)
        {
            if (id == null || status == null)
            {
                return NotFound();
            }

            var transport = await _bll.Transports.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (transport == null)
            {
                return NotFound();
            }

            await _bll.Transports.TransportActionAdd(transport, User.GetUserId()!.Value, status.Value);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            // var vm = new TransportCreateEditViewModel();
            // vm.Transport = transport;
            // vm.TransportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(transport.TransportNeedId);
            // vm.TransportOffer = await _bll.TransportOffers.FirstOrDefaultAsync(transport.TransportOfferId);
            //
            // vm.TransportOfferSelectList = new SelectList(await _bll.TransportOffers.GetAllAsync(), nameof(TransportOffer.Id), nameof(TransportOffer.Id), vm.Transport.TransportOfferId);
            // return View(vm);
        }

        // GET: Transports/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets Transport to edit by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportCreateEditViewModel</returns>
        public async Task<IActionResult> ModifyAndAdd(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _bll.Transports.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (transport == null)
            {
                return NotFound();
            }
            var transportNeed = await _bll.TransportNeeds.FirstOrDefaultAsync(transport.TransportNeedId);
            
            if (transportNeed == null)
            {
                return NotFound();
            }

            var transportOffer =
                await _bll.TransportOffers.FirstOrDefaultAsync(transport.TransportOfferId);
            if (transportOffer == null)
            {
                return NotFound();
            }
            var vm = new TransportCreateEditViewModel
            {
                TransportNeedId = transportNeed.Id, 
                TransportOfferId = transportOffer.Id,
                TransportOffer = transportOffer,
                TransportNeed = transportNeed,
                Transport = transport
            };
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
        public async Task<IActionResult> ModifyAndAdd(Guid id, TransportCreateEditViewModel vm)
        {
            if (id != vm.Transport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Transports.TransportSubmit(vm.Transport, vm.TransportOfferId, vm.TransportNeedId,  User.GetUserId()!.Value);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            
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
