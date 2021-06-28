using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.TransportNeeds;

namespace WebApp.Areas.Admin.Controllers.CrudControllers
{
    /// <summary>
    /// TransportNeed Controller 
    /// </summary>
    [Authorize]
    [Area("Admin")]
    public class TransportNeedsCrudController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// TransportNeed controller constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public TransportNeedsCrudController(IAppBLL bll)
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
            var res = await _bll.TransportNeeds.GetAllAsync();
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
        /// Create view. Gets TransportMeta and Organisation objects.
        /// </summary>
        /// <returns>View with TransportNeedCreateEditViewModel</returns>
        public async Task<IActionResult> Create()
        {
            var vm = new TransportNeedCrudCreateEditViewModel();
            vm.TransportMetaSelectList = new SelectList(await _bll.TransportMeta.GetAllAsync(User.GetUserId()!.Value), nameof(TransportMeta.Id), nameof(TransportMeta.Id));
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
        public async Task<IActionResult> Create(TransportNeedCrudCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.TransportNeed.AppUserId = User.GetUserId()!.Value;
                _bll.TransportNeeds.Add(vm.TransportNeed);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.TransportMetaSelectList = new SelectList(await _bll.TransportMeta.GetAllAsync(User.GetUserId()!.Value), nameof(TransportMeta.Id), nameof(TransportMeta.Id), vm.TransportNeed.TransportMetaId);
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

            var vm = new TransportNeedCrudCreateEditViewModel();
            vm.TransportNeed = transportNeed;
            vm.TransportMetaSelectList = new SelectList(await _bll.TransportMeta.GetAllAsync(User.GetUserId()!.Value), nameof(TransportMeta.Id), nameof(TransportMeta.Id), vm.TransportNeed.TransportMetaId);
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
        public async Task<IActionResult> Edit(Guid id, TransportNeedCrudCreateEditViewModel vm)
        {
            if (id != vm.TransportNeed.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid || ! await _bll.TransportNeeds.ExistsAsync(id, User.GetUserId()!.Value)) 
            {
                vm.TransportMetaSelectList = new SelectList(await _bll.TransportMeta.GetAllAsync(User.GetUserId()!.Value), nameof(TransportMeta.Id), nameof(TransportMeta.Id), vm.TransportNeed.TransportMetaId);
                return View(vm);
            }
            vm.TransportNeed.AppUserId = User.GetUserId()!.Value;
            _bll.TransportNeeds.Update(vm.TransportNeed);
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
            return await _bll.TransportNeeds.ExistsAsync(id,User.GetUserId()!.Value);
        }
    }
}
