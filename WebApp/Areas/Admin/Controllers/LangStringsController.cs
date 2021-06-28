using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.EF;
using Domain.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for langstrings
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LangStringsController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Langstrings controller constructor
        /// </summary>
        /// <param name="context"></param>
        public LangStringsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: LangStrings
        /// <summary>
        /// Index method
        /// </summary>
        /// <returns>Index page of langstrings</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.LangStrings.ToListAsync());
        }

        // GET: LangStrings/Details/5
        /// <summary>
        /// Details 
        /// </summary>
        /// <param name="id">Guid of langstring</param>
        /// <returns>Details view of Langstring</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appLangString = await _context.LangStrings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appLangString == null)
            {
                return NotFound();
            }

            return View(appLangString);
        }

        // GET: LangStrings/Create
        /// <summary>
        /// Create langstring initial
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: LangStrings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Save create langstring
        /// </summary>
        /// <param name="appLangString"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] AppLangString appLangString)
        {
            if (ModelState.IsValid)
            {
                appLangString.Id = Guid.NewGuid();
                _context.Add(appLangString);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appLangString);
        }

        // GET: LangStrings/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd langstring
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appLangString = await _context.LangStrings.FindAsync(id);
            if (appLangString == null)
            {
                return NotFound();
            }
            return View(appLangString);
        }

        // POST: LangStrings/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Saves edited langstring
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appLangString">AppLangString</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id")] AppLangString appLangString)
        {
            if (id != appLangString.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appLangString);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppLangStringExists(appLangString.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appLangString);
        }

        // GET: LangStrings/Delete/5
        /// <summary>
        /// Gets langstring to delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appLangString = await _context.LangStrings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appLangString == null)
            {
                return NotFound();
            }

            return View(appLangString);
        }

        // POST: LangStrings/Delete/5
        /// <summary>
        /// Delete langstring
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appLangString = await _context.LangStrings.FindAsync(id);
            _context.LangStrings.Remove(appLangString);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppLangStringExists(Guid id)
        {
            return _context.LangStrings.Any(e => e.Id == id);
        }
    }
}
