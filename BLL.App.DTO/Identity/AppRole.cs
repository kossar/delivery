using System;
using Domain.App.Identity;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;
using AppUserRole = Bll.App.DTO.Identity.AppUserRole;

namespace BLL.App.DTO.Identity
{
    public class AppRole: BaseRole<AppUserRole>
    {
        /*[StringLength(128, MinimumLength = 1)]
        public string DisplayName { get; set; } = default!;*/
    }
}