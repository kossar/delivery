using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Base.EF
{
    public class BaseDbContext<TUser, TRole, TUserRole, TUserClaim, TUserLogin, TRoleClaim, TUserToken, TLangString,
        TTranslation> : BaseDbContext<Guid, TUser, TRole, TUserRole, TUserClaim, TUserLogin, TRoleClaim, TUserToken,
        TLangString,
        TTranslation>
        where TUserClaim : IdentityUserClaim<Guid>
        where TUserLogin : IdentityUserLogin<Guid>
        where TRoleClaim : IdentityRoleClaim<Guid>
        where TUserToken : IdentityUserToken<Guid>
        where TUser : BaseUser<TUserRole>
        where TRole : BaseRole<TUserRole>
        where TUserRole : BaseUserRole<TUser, TRole>
        where TLangString : BaseLangString<TTranslation>, new()
        where TTranslation : BaseTranslation, new()
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    public class BaseDbContext<TKey, TUser, TRole, TUserRole, TUserClaim, TUserLogin, TRoleClaim, TUserToken,
        TLangString,
        TTranslation> : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TKey : IEquatable<TKey>
        where TUser : BaseUser<TKey, TUserRole>
        where TRole : BaseRole<TKey, TUserRole>
        where TUserRole : BaseUserRole<TKey, TUser, TRole>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
        where TLangString : BaseLangString<TKey, TTranslation>, new()
        where TTranslation : BaseTranslation<TKey>, new()
    {
        public DbSet<TLangString> LangStrings { get; set; } = default!;
        public DbSet<TTranslation> Translations { get; set; } = default!;

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // disable cascade delete initially for everything
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // enable cascade for langstring->translations
            builder.Entity<TLangString>().HasMany(x => x.Translations).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TTranslation>(b =>
            {
                // there is actually 1:m between langstring and translation
                b.HasOne<TLangString>().WithMany().HasForeignKey(ur => ur.LangStringId).IsRequired();
                // only one value allowed per culture/langstring
                b.HasIndex(x => new {x.Culture, x.LangStringId}).IsUnique();
            });


            // do not allow EF to create multiple FK-s, use existing RoleId and UserId
            builder.Entity<TUserRole>()
                .HasOne(x => x.User)
                .WithMany(x => x!.UserRoles)
                .HasForeignKey(x => x.UserId);

            builder.Entity<TUserRole>()
                .HasOne(x => x.Role)
                .WithMany(x => x!.UserRoles)
                .HasForeignKey(x => x.RoleId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) =>
            SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Delete the possible Langstrings when entity is deleted - there is no cascade delete from entity->langstring
            var entities = ChangeTracker.Entries().Where(entry => entry.State == EntityState.Deleted);
            foreach (var entity in entities.ToList())
            {
                var langStringsToDelete = new List<TLangString>();

                foreach (var reference in entity.References.Where(x => x.Metadata.FieldInfo.FieldType == typeof(TLangString) || x.Metadata.FieldInfo.FieldType.IsSubclassOf(typeof(BaseLangString<TKey, TTranslation>) )))
                {
                    var foreignKey = (reference.Metadata as INavigation)?.ForeignKey;
                    var fkPropertyName = foreignKey?.Properties
                        .Single().Name;
                    if (fkPropertyName != null)
                    {
                        if (reference.TargetEntry == null)
                        {
                            // get the fk value
                            var fkProperty = entity.Entity.GetType().GetProperties()
                                .First(x => x.Name == fkPropertyName);

                            var val = fkProperty.GetValue(entity.Entity);
                            if (val == null)
                            {
                                continue;
                            }
                            var fkValue = (TKey) val;
                            
                            // make up almost empty entity - we just need the id for delete
                            var fkEntity = new TLangString()
                            {
                                Id = fkValue
                            };
                            langStringsToDelete.Add(fkEntity);

                        }
                        else
                        {
                            if (reference.TargetEntry.Entity is TLangString langStringToDelete)
                            {
                                langStringsToDelete.Add(langStringToDelete);
                            }

                        }


                    }
                }

                // delete all found langstrings
                if (langStringsToDelete.Count > 0)
                {
                    Set<TLangString>().RemoveRange(langStringsToDelete);
                }
            }


            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}