using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Model for register confirmation
    /// </summary>
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _sender;

        /// <summary>
        /// RegisterConfirmationModel constructor
        /// </summary>
        /// <param name="userManager">UserManager</param>
        /// <param name="sender">IEmailSender</param>
        public RegisterConfirmationModel(UserManager<AppUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        /// <summary>
        /// Property for email address
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Property for displaying confirmation link
        /// </summary>
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>
        /// Property for email confirmation URl
        /// </summary>
        public string? EmailConfirmationUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">string?</param>
        /// <param name="returnUrl">string?</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(string? email, string? returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
