using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Base.Resources;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Index = Base.Resources.Areas.Identity.Pages.Account.Manage.Index;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Model for index page
    /// </summary>
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        /// <summary>
        /// IndexModel constructor
        /// </summary>
        /// <param name="userManager">UserManager</param>
        /// <param name="signInManager">SignInManager</param>
        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Property for Username 
        /// </summary>
        [Display(Name = nameof(Username), ResourceType = typeof(Index))]
        public string Username { get; set; } = default!;

        /// <summary>
        /// Property for status message
        /// </summary>
        [TempData] public string? StatusMessage { get; set; }

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
            /// Property for PhoneNumber
            /// </summary>
            [Phone(ErrorMessageResourceName = "ErrorMessage_NotValidPhone", ErrorMessageResourceType = typeof(Common))]
            [Display(Name = nameof(PhoneNumber), ResourceType = typeof(Index))]
            public string PhoneNumber { get; set; } = default!;
            
            /// <summary>
            /// Property for User FirstName
            /// </summary>
            [StringLength(128, ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", ErrorMessageResourceType = typeof(Common), MinimumLength = 1)]
            [Display(Name = nameof(FirstName), ResourceType = typeof(Index))]
            public string FirstName { get; set; } = default!;

            /// <summary>
            /// Property for user LastName
            /// </summary>
            [StringLength(128, ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", ErrorMessageResourceType = typeof(Common), MinimumLength = 1)]
            [Display(Name = nameof(LastName), ResourceType = typeof(Index))]
            public string LastName { get; set; } = default!;
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Page()</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(string.Format(Index.Unable_to_load_user_with_ID,_userManager.GetUserId(User)));
            }

            await LoadAsync(user);
            return Page();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Page()</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(string.Format(Index.Unable_to_load_user_with_ID,_userManager.GetUserId(User)));
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input!.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage =  Index.Unexpected_error_when_trying_to_set_phone_number;
                    return RedirectToPage();
                }
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
            {
                StatusMessage = Index.Unexpected_error_when_trying_to_update_profile_data;
                return RedirectToPage();
            }
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = Index.Your_profile_has_been_updated;
            return RedirectToPage();
        }
    }
}
