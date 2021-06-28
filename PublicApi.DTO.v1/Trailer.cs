using System;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1
{
    public class TrailerAdd
    {
        public int LoadCapacity { get; set; }

        [MaxLength(32)]
        public string RegNr { get; set; } = default!;
        
        public Guid UnitId { get; set; }
        
        public Guid DimensionsId { get; set; }
    }

    public class Trailer
    {
        public Guid Id { get; set; }

        public int LoadCapacity { get; set; }
        
        public string RegNr { get; set; } = default!;

        public Dimensions? Dimensions { get; set; }
        
        public Guid UnitId { get; set; }
    }
}