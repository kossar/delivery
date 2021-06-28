using System;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;

namespace Domain.Base
{
    public class BaseTranslation : BaseTranslation<Guid>, IDomainEntityId
    {
    }

    /// <summary>
    /// Translations for LangString
    /// Using composite PK from Culture & LangStringId, defined in basedbcontext
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TLangString"></typeparam>
    public class BaseTranslation<TKey>: DomainEntityId<TKey> 
        where TKey : IEquatable<TKey>
    {
        [MaxLength(5)]
        public virtual string Culture { get; set; } = default!;

        [MaxLength(10240)]
        public virtual string Value { get; set; } = "";

        public virtual TKey LangStringId { get; set; } = default!;        
    }

}