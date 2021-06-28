
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Model for showing recovery codes
    /// </summary>
    public class ShowRecoveryCodesModel : PageModel
    {
        /// <summary>
        /// Array of strings property for recovery codes
        /// </summary>
        [TempData] public string[]? RecoveryCodes { get; set; }

        /// <summary>
        /// Property for StatusMessage
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Page()</returns>
        public IActionResult OnGet()
        {
            if (RecoveryCodes == null || RecoveryCodes.Length == 0)
            {
                return RedirectToPage("./TwoFactorAuthentication");
            }

            return Page();
        }
    }
}
