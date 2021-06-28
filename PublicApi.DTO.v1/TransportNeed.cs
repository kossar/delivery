using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PublicApi.DTO.v1.Enums;

namespace PublicApi.DTO.v1
{
    public class TransportNeedAdd
    {
        [MaxLength(1024)]
        public string? TransportNeedInfo { get; set; }

        public ETransportType TransportType { get; set; }

        public int PersonCount { get; set; }
        
        public Guid TransportMetaId { get; set; }
    }
    public class TransportNeed
    {
        public Guid Id { get; set; }

        public Guid AppUserId { get; set; }
        
        [MaxLength(1024)]
        public string? TransportNeedInfo { get; set; }

        public ETransportType TransportType { get; set; }

        public int PersonCount { get; set; }

        public List<Guid>? ParcelIds { get; set; }
        public TransportMeta TransportMeta { get; set; } = default!;
    }
}