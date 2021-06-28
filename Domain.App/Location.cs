using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.App
{
    public class Location : DomainEntityId
    {
        public Guid CountryId { get; set; }
        [InverseProperty(nameof(AppLangString.Country))]
        public virtual AppLangString? Country { get; set; }

        public Guid CityId { get; set; }
        [InverseProperty(nameof(AppLangString.City))]
        public virtual AppLangString? City { get; set; }

        public Guid AddressId { get; set; }
        [InverseProperty(nameof(AppLangString.Address))]
        public virtual AppLangString? Address { get; set; }


        public Guid? LocationInfoId { get; set; }
        [InverseProperty(nameof(AppLangString.LocationInfo))]
        public virtual AppLangString? LocationInfo { get; set; }

        public ICollection<TransportMeta>? StartLocations { get; set; }
        public ICollection<TransportMeta>? DestinationLocations { get; set; }

        public ICollection<Transport>? Transports { get; set; }
    }
}