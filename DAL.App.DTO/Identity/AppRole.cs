using System;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;

namespace DAL.App.DTO.Identity
{
    public class AppRole: BaseRole<AppUserRole>
    {
        /*[StringLength(128, MinimumLength = 1)]
        public string DisplayName { get; set; } = default!;*/
    }
}