using System;

namespace PublicApi.DTO.v1
{
    public class TransportMetaAdd
    {
        public Guid StartLocationId { get; set; }

        public Guid DestinationLocationId { get; set; }

        public DateTime StartTime { get; set; }
    }

    public class TransportMeta
    {
        public Guid Id { get; set; }

        public Location StartLocation { get; set; } = default!;

        public Location DestinationLocation { get; set; } = default!;
        
        public DateTime StartTime { get; set; }
    }
}