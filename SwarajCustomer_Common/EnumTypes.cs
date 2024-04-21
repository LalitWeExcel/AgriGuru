namespace SwarajCustomer_Common
{
    public enum Roles
    {
        CUST = 1,   // Customer
        AST = 2,    // Astrologer
        PRHT = 3,   // Purohit
        ADMIN = 4,   // admin
        PPRHT = 5,   // Primum  Purohit
        PAST = 6,   // Primum Astrologer
    }

    public enum AppType
    {
        CUST,       // Customer App
        AST ,    // Astrologer App
        PRHT ,  // Purohit App
        ADMIN  ,  // admin
        PPRHT ,   // Primum  Purohit
        PAST ,   // Primum Astrologer
    }

    public enum PujaCategory
    {
        PUJA = 1,   // Puja
        PATH = 2,   // Path
        CORP = 3,   // Corporate Puja
        COMBOSPACKAGESERVCIE = 1005,   // CombosPackagesService 
    }

    public enum SamagriCategory
    {
        PUJA = 1,   // Puja
        ANYA = 2,   // Path
        GHAR = 3,   // Corporate Puja
        COMBOSPACKAGESERVCIE = 1005,   // CombosPackagesService 
        
    }

    public enum ConsultationMediumEnum
    {
        HVST = 1,   // Home Visit
        PCAL = 2,   // Phone Call
        VCAL = 3,   // Video Call
    }

    public enum AddToCarEnum
    {
        SavePujaOrder = 1,   // Save Puja Order api
        SaveAstroOrder = 2,   // Save Astro Order  api
        SavePackageOrder = 3,   // Save package servie Order  api
        SaveIndependentOrder=4

    }

    public enum ContentType
    {
        SPO = 1,      // Save Puja Order
        SAO = 2,      // Save Astro Order
        MBC = 3,       //Booking Cancel
        SPSO = 4,      //Save package servie Order
        PACBAFB = 5,   //Prohit/Astro  Confirm By Admin for booking
        ABFNPABA = 6,   //Assign booking for new Prohit/Astrologer  Because  old prophet/Astrologer rejects this booking or unable for this booking!!
         AMPFSPB = 7,   //Assign multiple Prohit for service package booking Because the old prohit rejects this booking or unable for this booking or no any one select prohit select by admin !!
        BC = 8 ,      //Booking Confirm
        SIA = 9       //Save Independent Ads
    }
    public enum BookingType
    {
        Purohit = 1,   // Save Purohit
        Astrologer = 2,   // Save Astrologer
        Service = 3,   //save Service
    }
}
