using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Model for resetting password
    /// </summary>
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// ResetPasswordModel constructor
        /// </summary>
        /// <param name="userManager">UserManager</param>
        public ResetPasswordModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Property for InputModel
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = default!;

        /// <summary>
        /// InputModel class
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Property for Email
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(Base.Resources.Common), ErrorMessageResourceName = "ErrorMessage_Required")]
            [EmailAddress(ErrorMessageResourceType = typeof(Base.Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_Email")]
            public string? Email { get; set; }

            /// <summary>
            /// Propery for password
            /// </summary>
            [Required(ErrorMessageResourceType = typeof(Base.Resources.Common), ErrorMessageResourceName = "ErrorMessage_Required")]
            [StringLength(100, ErrorMessageResourceType = typeof(Base.Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", MinimumLength = 6)]
            [DataType(DataType.Password)]
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
            /// Property for code
            /// </summary>
            public string? Code { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">string?</param>
        /// <returns></returns>
        public IActionResult OnGet(string? code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input!.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
