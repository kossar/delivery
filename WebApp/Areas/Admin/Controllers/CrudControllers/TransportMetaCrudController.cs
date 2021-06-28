using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels.TransportMetas;

namespace WebApp.Areas.Admin.Controllers.CrudControllers
{
    /// <summary>
    /// TransportMeta Controller.
    /// </summary>
    [Area("Admin")]
    public class TransportMetaCrudController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// TransportMetaCrudController constructor.
        /// </summary>
        /// <param name="bll">IAppBLL</param>
        public TransportMetaCrudController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: TransportMeta
        /// <summary>
        /// Index view. Gets All TransportMeta objects.
        /// </summary>
        /// <returns>List of TransportMeta objects</returns>
        public async Task<IActionResult> Index()
        {
            var res = await _bll.TransportMeta.GetAllAsync();
            return View(res);
        }

        // GET: TransportMeta/Details/5
        /// <summary>
        /// Details view. Gets TransportMeta object by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportMeta object.</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportMeta = await _bll.TransportMeta.FirstOrDefaultAsync(id.Value);
            
            if (transportMeta == null)
            {
                return NotFound();
            }

            return View(transportMeta);
        }

        // GET: TransportMeta/Create
        /// <summary>
        /// Create view. Gets Locations for TransportMeta.
        /// </summary>
        /// <returns>View with TransportMetaCreateEditViewModel</returns>
        public async Task<IActionResult> Create()
        {
            var vm = new TransportMetaCreateEditViewModel();
            vm.DestinationLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(User.GetUserId()!.Value), nameof(Location.Id), nameof(Location.City));
            vm.StartLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(User.GetUserId()!.Value), nameof(Location.Id), nameof(Location.City));
            return View(vm);
        }

        // POST: TransportMeta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Create view. Saces new TransportMeta object.
        /// </summary>
        /// <param name="vm">TransportMetaCreateEditViewModel</param>
        /// <returns>View with TransportMetaCreateEditViewModel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransportMetaCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.TransportMeta.Add(vm.TransportMeta);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.DestinationLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(User.GetUserId()!.Value), nameof(Location.Id), nameof(Location.City), vm.TransportMeta.DestinationLocationId);
            vm.StartLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(User.GetUserId()!.Value), nameof(Location.Id), nameof(Location.City), vm.TransportMeta.StartLocationId);
            return View(vm);
        }

        // GET: TransportMeta/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd view. Gets TransportMeta to edit by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportMetaCreateEditViewModel</returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportMeta = await _bll.TransportMeta.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (transportMeta == null)
            {
                return NotFound();
            }

            var vm = new TransportMetaCreateEditViewModel();
            vm.TransportMeta = transportMeta;
            
            vm.DestinationLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(User.GetUserId()!.Value), nameof(Location.Id), nameof(Location.City), vm.TransportMeta.DestinationLocationId);
            vm.StartLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(User.GetUserId()!.Value), nameof(Location.Id), nameof(Location.City), vm.TransportMeta.StartLocationId);
            return View(vm);
        }

        // POST: TransportMeta/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// ModifyAndAdd view. Updates edited TransportMeta object.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="vm">TransportMetaCreateEditViewModel</param>
        /// <returns>Redirects to index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TransportMetaCreateEditViewModel vm)
        {
            if (id != vm.TransportMeta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.TransportMeta.Update(vm.TransportMeta);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            vm.DestinationLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(User.GetUserId()!.Value), nameof(Location.Id), nameof(Location.City), vm.TransportMeta.DestinationLocationId);
            vm.StartLocationSelectList = new SelectList(await _bll.Locations.GetAllAsync(User.GetUserId()!.Value), nameof(Location.Id), nameof(Location.City), vm.TransportMeta.StartLocationId);
            return View(vm);
        }

        // GET: TransportMeta/Delete/5
        /// <summary>
        /// Delete view. Gets TransportMeta to delete by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with TransportMeta object.</returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportMeta = await _bll.TransportMeta.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (transportMeta == null)
            {
                return NotFound();
            }

            return View(transportMeta);
        }

        // POST: TransportMeta/Delete/5
        /// <summary>
        /// Delete view. Deletes TransportMeta by id.
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>Redirects to Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.TransportMeta.RemoveAsync(id, User.GetUserId()!.Value);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TransportMetaExists(Guid id)
        {
            return await _bll.TransportMeta.ExistsAsync(id, User.GetUserId()!.Value);
        }
    }
}
