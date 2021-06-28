using System;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1
{
    public class UnitAdd
    {
        [MaxLength(8)]
        public string UnitCode { get; set; } = default!;
        
        [MaxLength(32)]
        public string UnitName { get; set; } = default!;
    }

    public class Unit : UnitAdd
    {
        public Guid Id { get; set; }
    }
}