using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Static class for managing navigation pages
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// Static string for Index
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// Static string for Email
        /// </summary>
        public static string Email => "Email";

        /// <summary>
        /// Static string ChangePassword
        /// </summary>
        public static string ChangePassword => "ChangePassword";

        /// <summary>
        /// Static string DownloadPersonalData
        /// </summary>
        public static string DownloadPersonalData => "DownloadPersonalData";

        /// <summary>
        /// Static string DeletePersonalData
        /// </summary>
        public static string DeletePersonalData => "DeletePersonalData";

        /// <summary>
        /// Static string ExternalLogins
        /// </summary>
        public static string ExternalLogins => "ExternalLogins";

        /// <summary>
        /// Static string PersonalData
        /// </summary>
        public static string PersonalData => "PersonalData";

        /// <summary>
        /// Static string TwoFactorAuthentication
        /// </summary>
        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        /// <summary>
        /// Index navigation class
        /// </summary>
        /// <param name="viewContext">ViewContext</param>
        /// <returns></returns>
        public static string? IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext">ViewContext</param>
        /// <returns></returns>
        public static string? EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext">ViewContext</param>
        /// <returns></returns>
        public static string? ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext">ViewContext</param>
        /// <returns></returns>
        public static string? DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext">ViewContext</param>
        /// <returns></returns>
        public static string? DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext">ViewContext</param>
        /// <returns></returns>
        public static string? ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext">ViewContext</param>
        /// <returns></returns>
        public static string? PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext">ViewContext</param>
        /// <returns></returns>
        public static string? TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        private static string? PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return (string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null);
        }
    }
}
