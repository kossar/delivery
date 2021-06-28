using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Base.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace WebApp.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Model for registering
    /// </summary>
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// RegisterModel constructor
        /// </summary>
        /// <param name="userManager">UserManager</param>
        /// <param name="signInManager">SignInManager</param>
        /// <param name="logger">ILogger</param>
        /// <param name="emailSender">IEmailSender</param>
        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Property for input model
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = default!;

        /// <summary>
        /// Property for return URL
        /// </summary>
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// Property for external logins
        /// </summary>
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        /// <summary>
        /// InputModel class
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Property for Email
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [EmailAddress(ErrorMessageResourceType = typeof(Base.Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_Email")]
            [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
                Name = nameof(Email))]
            public string? Email { get; set; }

            /// <summary>
            /// Property for password
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [StringLength(100, ErrorMessageResourceType = typeof(Base.Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = nameof(Password),
                ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register))]
            public string? Password { get; set; }

            /// <summary>
            /// Property for password confirmation
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = nameof(ConfirmPassword),
                ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register))]
            [Compare("Password",
                ErrorMessageResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
                ErrorMessageResourceName = "PasswordsDontMatch")]
            public string? ConfirmPassword { get; set; }

            /// <summary>
            /// Property for user FirstName
            /// </summary>
            [MaxLength(128, ErrorMessageResourceType = typeof(Base.Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")] 
            [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
                Name = nameof(FirstName))]
            public string? FirstName { get; set; }
            
            /// <summary>
            /// Property for user Phonenumber
            /// </summary>
            [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register), 
                Name = nameof(PhoneNumber))]
            [Phone(ErrorMessageResourceName = "ErrorMessage_NotValidPhone", ErrorMessageResourceType = typeof(Common))]
            [DataType(DataType.PhoneNumber)]
            public string? PhoneNumber { get; set; }

            /// <summary>
            /// Property for user LastName
            /// </summary>
            [MaxLength(128, ErrorMessageResourceType = typeof(Base.Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")] 
            [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
                Name = nameof(LastName))]
            public string? LastName { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl">string?</param>
        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl">string?</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName!,
                    LastName = Input.LastName!,
                    PhoneNumber = Input.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
