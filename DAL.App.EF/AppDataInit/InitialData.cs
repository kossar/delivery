using System;
using Domain.App;
using Domain.App.Enums;

namespace DAL.App.EF.AppDataInit
{
    public static class InitialData
    {
        public static readonly (string roleName, string roleDisplayName, Guid id)[] Roles =
        {
            ("Admin", "Administrators", Guid.Parse("00000000-1000-0000-0001-000000000000")),
        };


        public static readonly (string name, string password, string firstName, string lastName, Guid id, string role)[]
            Users =
            {
                ("admin@otkoss.com", ".Tere123", "Admin", "Demo",
                    Guid.Parse("10000000-0000-0000-0000-000000000001"), "Admin"),
                ("user@demo.ee", "Foo.bar1", "User", "Demo",
                    Guid.Parse("10000000-0000-0000-0000-000000000002"), ""),
            };
        
        public static readonly (string unitCode, string unitName, EUnitType)[]
            Units =
            {
                ("kg", "Kilogram", EUnitType.Weight),
                ("g", "Gram", EUnitType.Weight),
                ("t", "Ton", EUnitType.Weight),
                ("m", "Meter", EUnitType.Length),
                ("cm", "Centimeter", EUnitType.Length),
                ("mm", "millimeter", EUnitType.Length)
                
            };

        public static readonly (string country, string city, string address, string? locationinfo)[]
            Locations =
            {
                ("Estonia", "Tallinn", "Karu 2-6", "White door in back of house"),
                ("Estonia", "Tallinn", "Peterburi tee 14", "3rd floor"),
                ("Estonia", "Tallinn", "Akadeemia tee 14-2", null),
                ("Estonia", "Narva", "Kangelaste prospekt 14", "Turn left after parking lot"),
                ("Estonia", "Rakvere", "Pikk 11", null),
                ("Estonia", "Tartu", "Ülikooli 1", "Esimene korrus"),
                ("Estonia", "Pärnu", "Vana 17", "Ei oska midagi öelda"),
                ("Estonia", "Paide", "Prääma 9", "3rd floor"),
                ("Estonia", "Jõgeva", "Labisoidu 11", null),
                ("Finland", "Helsinki", "Haamentie 8-14", "Miskit siia ka"),
                ("Latvia", "Riga", "Lacpecas iela 8", "Lv lan txt"),
            };
        
        public static readonly (decimal width, decimal height, decimal length)[]
            DimensionsMeters =
            {
                (2m, 1.8m, 3.1m),
                (2m, 2m, 3m),
                (1m, 2m, 4m),
                (3m, 1m, 3m)
            };
        public static readonly (decimal width, decimal height, decimal length)[]
            DimensionsMm =
            {
                (2000m, 1800m, 3100m),
                (2000m, 2000m, 3000m),
                (100m, 220m, 412m),
                (70m, 66m, 140m)
            };
        
        public static readonly (string name, bool forGoods)[]
            VehicleTypes =
            {
                ("Car", false),
                ("Truck", true),
                ("Van", true),
            };
        
        public static readonly (string info, ETransportType transportType, int personCount, TransportMeta meta)[]
            TransportNeeds =
            {
                ("Car", ETransportType.ParcelsOnly, 2, new TransportMeta()
                {
                   StartTime = DateTime.Now.AddDays(2),
                   StartLocation = new Location()
                   {
                       Country = "Est",
                       City = "Tallinn",
                       Address = "xx-2"
                   },
                   DestinationLocation = new Location()
                   {
                       Country = "Est",
                       City = "tartu",
                       Address = "xx-2"
                   }
                }),
            };
    }
}