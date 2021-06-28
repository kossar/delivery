using System;

namespace PublicApi.DTO.v1
{
    public class DimensionsAdd
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        
        public decimal Length { get; set; }

        public Guid UnitId { get; set; }
    }
    public class Dimensions
    {
        public Guid Id { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        
        public decimal Length { get; set; }

        public string Whl => Width + " x " + Height + " x " + Length + " " + UnitCode;

        public Guid UnitId { get; set; }

        public string? UnitCode { get; set; }
    }
}