using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using Microsoft.AspNetCore.Identity;
using BaseResources = Base.Resources;

namespace Domain.Base.Identity
{
    public class BaseUser<TUserRole> : BaseUser<Guid, TUserRole>, IDomainEntityId
        where TUserRole : IdentityUserRole<Guid>
    {
    }

    public class BaseUser<TKey, TUserRole> : IdentityUser<TKey>, IDomainEntityId<TKey>
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>
    {
        [Display(Name = "FirstName", ResourceType = typeof(BaseResources.Base.Domain.Identity.BaseUser))]
        [MaxLength(128)]
        public virtual string FirstName { get; set; } = default!;

        [Display(Name = "LastName", ResourceType = typeof(BaseResources.Base.Domain.Identity.BaseUser))]
        [MaxLength(128)]
        public virtual string LastName { get; set; } = default!;


        public virtual ICollection<TUserRole>? UserRoles { get; set; }
        public virtual string FullName => FirstName + " " + LastName;
        public virtual string FullNameEmail => FullName + " (" + Email + ")";


        [Display(Name = "FirstLastName",
            ResourceType = typeof(BaseResources.Base.Domain.Identity.BaseUser))]
        public virtual string FirstLastName => FirstName + " " + LastName;

        [Display(Name = "LastFirstName",
            ResourceType = typeof(BaseResources.Base.Domain.Identity.BaseUser))]
        public virtual string LastFirstName => LastName + " " + FirstName;
    }
}