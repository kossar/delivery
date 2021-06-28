using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.EF;
using Domain.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for translations
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TranslationsController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Translation controller constructor
        /// </summary>
        /// <param name="context">AppDbContext</param>
        public TranslationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Translations
        /// <summary>
        /// Get all translations
        /// </summary>
        /// <returns>View with translations</returns>
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Translations.Include(a => a.LangString);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Translations/Details/5
        /// <summary>
        /// Translation details
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns>View with translation</returns>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appTranslation = await _context.Translations
                .Include(a => a.LangString)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appTranslation == null)
            {
                return NotFound();
            }

            return View(appTranslation);
        }

        // GET: Translations/Create
        /// <summary>
        /// Gets langstrings and returns view to create translation
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewData["LangStringId"] = new SelectList(_context.LangStrings, "Id", "Id");
            return View();
        }

        // POST: Translations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Save created translation
        /// </summary>
        /// <param name="appTranslation"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Culture,Value,LangStringId,Id")] AppTranslation appTranslation)
        {
            if (ModelState.IsValid)
            {
                appTranslation.Id = Guid.NewGuid();
                _context.Add(appTranslation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LangStringId"] = new SelectList(_context.LangStrings, "Id", "Id", appTranslation.LangStringId);
            return View(appTranslation);
        }

        // GET: Translations/ModifyAndAdd/5
        /// <summary>
        /// ModifyAndAdd Translation
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appTranslation = await _context.Translations.FindAsync(id);
            if (appTranslation == null)
            {
                return NotFound();
            }
            ViewData["LangStringId"] = new SelectList(_context.LangStrings, "Id", "Id", appTranslation.LangStringId);
            return View(appTranslation);
        }

        // POST: Translations/ModifyAndAdd/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Save edited translation
        /// </summary>
        /// <param name="id">GUID</param>
        /// <param name="appTranslation">AppTranslation</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Culture,Value,LangStringId,Id")] AppTranslation appTranslation)
        {
            if (id != appTranslation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appTranslation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppTranslationExists(appTranslation.Id))
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
            ViewData["LangStringId"] = new SelectList(_context.LangStrings, "Id", "Id", appTranslation.LangStringId);
            return View(appTranslation);
        }

        // GET: Translations/Delete/5
        /// <summary>
        /// GEts translation to delete
        /// </summary>
        /// <param name="id">GUID </param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appTranslation = await _context.Translations
                .Include(a => a.LangString)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appTranslation == null)
            {
                return NotFound();
            }

            return View(appTranslation);
        }

        // POST: Translations/Delete/5
        /// <summary>
        /// Deletes translation
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appTranslation = await _context.Translations.FindAsync(id);
            _context.Translations.Remove(appTranslation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppTranslationExists(Guid id)
        {
            return _context.Translations.Any(e => e.Id == id);
        }
    }
}
