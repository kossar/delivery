using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace PublicApi.DTO.v1
{
    public class VehicleAdd
    {
        [MaxLength(32)]
        public string Make { get; set; } = default!;
        [MaxLength(32)]
        public string Model { get; set; } = default!;
        
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [MaxLength(32)]
        public string RegNr { get; set; } = default!;
        public Guid VehicleTypeId { get; set; }
    }

    public class Vehicle : VehicleAdd
    {
        public Guid Id { get; set; }

        public string Age { get; set; } = default!;
    }
}