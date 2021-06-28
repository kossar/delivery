using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;

namespace Domain.App.Identity
{
    public class AppRole: BaseRole<AppUserRole>
    {
        /*[StringLength(128, MinimumLength = 1)]
        public string DisplayName { get; set; } = default!;*/
    }
}