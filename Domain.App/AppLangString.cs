using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.App
{
    public class AppLangString: BaseLangString<AppTranslation>
    {
        // navigation properties back where this string is used
        // since every string is unique, only one nav property should have value
        // no fk-s here, keys are kept on the other end of relationship
        public Location? Country { get; set; }
        public Location? City { get; set; }
        public Location? Address { get; set; }
        public Location? LocationInfo { get; set; }
        public Parcel? ParcelInfo { get; set; }

        public TransportNeed? TransportNeedInfo { get; set; }
        
        public TransportOffer? TransportOfferInfo { get; set; }

        public Unit? UnitCode { get; set; }
        public Unit? UnitName { get; set; }

        public Vehicle? Make { get; set; }
        public Vehicle? Model { get; set; }

        public VehicleType? VehicleTypeName { get; set; }
        public AppLangString(): base()
        {
            
        }

        public AppLangString(string value, string? culture = null) : base(value, culture)
        {
            
        }
        public static implicit operator string(AppLangString? l) => l?.ToString() ?? "null";

        // LangString s = "Foo"
        public static implicit operator AppLangString(string s) => new AppLangString(s);

        public override string ToString()
        {
            return base.ToString();
        }
    }
    
    
}