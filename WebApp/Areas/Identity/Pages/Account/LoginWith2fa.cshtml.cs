using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Areas.Identity.Pages.Account
{
    /// <summary>
    /// 2 factor authentication Login model
    /// </summary>
    [AllowAnonymous]
    public class LoginWith2FaModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginWith2FaModel> _logger;

        /// <summary>
        /// LoginWith2faModel constructor
        /// </summary>
        /// <param name="signInManager">SignInManager</param>
        /// <param name="logger">ILogger</param>
        public LoginWith2FaModel(SignInManager<AppUser> signInManager, ILogger<LoginWith2FaModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Input model property
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = default!;

        /// <summary>
        /// Boolean to remember user
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Url to return
        /// </summary>
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// Input Model class
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// 2FA code
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(Base.Resources.Common), ErrorMessageResourceName = "ErrorMessage_Required")]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; } = default!;

            /// <summary>
            /// Boolean to remember this machine
            /// </summary>
            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rememberMe">bool</param>
        /// <param name="returnUrl">string?</param>
        /// <returns>Page()</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IActionResult> OnGetAsync(bool rememberMe, string? returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rememberMe">bool</param>
        /// <param name="returnUrl">string?</param>
        /// <returns>Page()</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IActionResult> OnPostAsync(bool rememberMe, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl ??= Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = Input!.TwoFactorCode!.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa", user.Id);
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return Page();
            }
        }
    }
}
