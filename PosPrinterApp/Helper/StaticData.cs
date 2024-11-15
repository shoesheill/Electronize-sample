using TicketApp.Helper;

namespace PosPrinterApp.Helper
{
    internal class StaticData
    {
        public static string LogoPath="logo.png";
        public static int TicketCount = 0;
        public static bool IsCCMS = true;
        public static int ID=0;
        public static float ThreeDCharge = 50;
        public static bool EnableTaxRegistration = false;
        public static bool IsComplimentary = false;
        public static float BoxOfficeTax = 0.15F;
        public static float LocalTax = 0.05F;
        public static float LocalTaxInternational = 0.05F;
        public static float FDF = 0.15F;
        public static float EntertainmentTax = 0.13F;
        public static float ConvinienceCharge = 0;
        public static string PrintType = "barcode";
        public static string TermsAndConditions = "Terms & Conditions.\n" +
                            "1. Tickets once sold can not be refunded\n" +
                            "2. Lost, stolen or damaged tickets will not be replaced\n" +
                            "3. Seat allocation cannot be altered after the purchase of the tickets.\n";
        public static List<TicketDetails> lstTicket = new List<TicketDetails>()
        {
            new TicketDetails()
            {
                ID=1,
                Title="No Data Availabe",
                Time=DateTime.Now.ToString("HH:mm tt"),
                Price=0
            }
        };
    }
}
