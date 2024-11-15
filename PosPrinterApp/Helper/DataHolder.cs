using System.Drawing;
using System.Net.NetworkInformation;

namespace PosPrinterApp.Helper
{
    public class DataHolder
    {
        public const string AlphaNumericTextGenerator = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public const string BookedOrderKey = "BookedOrder";
        public const string BookedSeatsKey = "BookedSeat";
        public const string BookedTicketKey = "BookedTickets";
        public const int CardCodeLength = 6;
        public const string CompaniesTermAndConditions = "Big data to feed machine learning also became cheaper and easier Modern machine learning became what it is thanks to the rise of fast-moving, readily available, and cheaply stored data.Almost everyone with a hand in machine learning also had a hand in feeding it big data -- for instance, IBM's Watson and its Internet of things APIs. IBM in particular cut deals with third-party data providers like the Weather Channel to have even more sources of data that its customers can feed to their machine-learning-powered solutions. ";

        public const int PortalID = 1;
        public const string RandomTextGenerator = "0123456789";
        public const string SelectedShowKey = "SelectedShow";
        public const int TheatreID = 1;
        public static int TheaterID = 1;
        public const string TicketFoodMapKey = "TicketFoodMap";
        public static string APIURL = "net.tcp://localhost:1003/CineRetroStaffService.svc";
        public static Image AppLogo = null;
        public static string BookingNumber = string.Empty;
        public static string HallAddress = "Ratopul, Kathmandu - Nepal";
        public static string HallName = "Sage Flick";
        public static string defaultTAC =
                          "Terms & Conditions.\n" +
                           "1. Tickets once sold can not be refunded\n" +
                           "2. Lost, stolen or damaged tickets will not be replaced\n" +
                           "3. Seat allocation cannot be altered after the purchase of the tickets.\n" +
                           "4. " + HallName + " reserves the right to cancel a show, substitute an alternative\n" +
                           "film or change the timings of the show if necessary for any reason.\n" +
                           "5. Viewers are advised to take care of their belongings and goods,\n" +
                           "any damaged or stolen goods cannot be claim with " + HallName + ".\n" +
                           "6. If a film showing is cancelled or its timing is altered, the cost of purchase of\n" +
                           "the tickets will either be refunded or replacement tickets for the same show at " + "alternative ShowTime will be issued.";
        public static string HostURL = string.Empty;

        public static bool IsTermsAndCondition = false;
        public static Color PrimaryButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(120)))), ((int)(((byte)(23)))));
        //public static Color SecondaryButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(120)))), ((int)(((byte)(23)))));
        public static void SetTAC()
        {
            if (!DataHolder.IsTermsAndCondition)
                DataHolder.defaultTAC = "Terms & Conditions.\n" +
                               "1. Tickets once sold can not be refunded\n" +
                               "2. Lost, stolen or damaged tickets will not be replaced\n" +
                               "3. Seat allocation cannot be altered after the purchase of the tickets.\n" +
                               "4. " + HallName + " reserves the right to cancel a show, substitute an alternative\n" +
                               "film or change the timings of the show if necessary for any reason.\n" +
                               "5. Viewers are advised to take care of their belongings and goods,\n" +
                               "any damaged or stolen goods cannot be claim with " + HallName + ".\n" +
                               "6. If a film showing is cancelled or its timing is altered, the cost of purchase of\n" +
                               "the tickets will either be refunded or replacement tickets for the same show at " + "alternative ShowTime will be issued.";
        }
        public static Size ImageSize = new Size(18, 18);
        public static string ImageString = "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AAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEBHYAgAAAAAdASGAAAAAEDn/wUYAN2r1FBhoLhIAAAAAElFTkSuQmCC";
        public static bool isKitchenCopy = false;
        public static bool isQuickOrder = false;
        public static bool isReprintEnabled = false;
        public static int MaxSeatBooking = 5;

        //For Images
        public static string HDMovieImagePath = "/Modules/CineUploadFiles/Movie/HDImage/";
        public static string MenuItemImagePath = "/Modules/CineUploadFiles/MenuItem/image/Thumbnails/Large/";
        public static string MovieImagePath = "/Modules/CineUploadFiles/Movie/Image/Thumbnails/large/";
        public static string PackageImagePath = "/Modules/CineUploadFiles/Package/image/Thumbnails/Large/";

        public static int NavigationMode = 1;

        public static Image PrintLogo = null;
        public static string SeatNumber = string.Empty;
        public static int SelectedIndex = 0;
        public static decimal ServiceTaxRate = 0;
        public static DateTime StartTime;

        //1 for Ticketfoodmapping, 2 for Booking Verify, 3 for Resturant
        public static int TempNavigationMode = 1;

        public static int TempPackageID = -1;


        public static string TermsAndCondition = "Terms & Conditions.\n" +
                            "1. Tickets once sold can not be refunded\n" +
                            "2. Lost, stolen or damaged tickets will not be replaced\n" +
                            "3. Seat allocation cannot be altered after the purchase of the tickets.\n";


        public static string TermsAndConditions = "Terms & Conditions.\n" +
                            "1. Tickets once sold can not be refunded\n" +
                            "2. Lost, stolen or damaged tickets will not be replaced\n" +
                            "3. Seat allocation cannot be altered after the purchase of the tickets.\n" +
                            "4. " + DataHolder.HallName + " reserves the right to cancel a show, substitute an alternative\n" +
                            "film or change the timings of the show if necessary for any reason.\n" +
                            "5. Viewers are advised to take care of their belongings and goods,\n" +
                            "any damaged or stolen goods cannot be claim with " + DataHolder.HallName + ".\n" +
                            "6. If a film showing is cancelled or its timing is altered, the cost of purchase of\n" +
                            "the tickets will either be refunded or replacement tickets for the same show at " + "alternative ShowTime will be issued.";


        public static decimal ThreeDCharge = 0;

        public static string ThreeDMessage = "All glasses HAVE TO BE RETURNED after the show.\n" +
                                          "Rs. 200 will be fined for DAMAGED or LOST glasses. HAVE FUN!";

        public static string TicketNumber = string.Empty;
        public static string UserName = string.Empty;
        public static int UserType = 0;
        //public static string VatNo = "0000000000";
        public static decimal VatRate = 13;
        public static string RewardPositiveResponse = "Congratulations!!! Your accumulated reward point is {0}. \nYou are now eligible for complimentary tickets.";
        public static string RewardNegativeResponse = "Your accumulated reward point is {0}. \nYou do not have enough reward points for complimentary tickets.";
        public static int RewardApplicableForTicket = 1;
        public static int RewardApplicableForFood = 2;
        public static string InclusiveTaxResponse = "Inclusive of Government Tax {0}";
        public static string CardNotRegistered = "Your card is not registered yet.";
        public static string CardNotActive = "Your card not active in the system. Please contact to administrator.";

        public static string TheaterVatNo = "00TheaterVAT00";
        public static string CafeVatNo = "00FoodVAT00";

        public static string HallCompanyName = "Sageflick Pvt. Ltd.";

        public static string MaxTicketForMemberOnDay = "Maximum {0} tickets for members on a day.\n";
        public static string AvailableTicketsOnDay = "{0} out of {1} tickets available for today.\n";
        public static string TicketLimitExceedToday = "Your reached your ticket limit for today\n";

        public static string CardNotFound = "Card is either not registered or not active in the system";
        public static string CardNotEligible = "Card is eligible for the selected day";

        public static bool ShowDualMoniter = true;

        public static string GetMacAddress()
        { return US.MacAddress; }

        //temp values
        //public static void SetMacAddress()
        //{
        //    string macAddress = string.Empty;
        //    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //    PhysicalAddress address = nics[0].GetPhysicalAddress();
        //    byte[] bytes = address.GetAddressBytes();
        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        macAddress = macAddress + string.Format("{0}", bytes[i].ToString("X2"));
        //        if (i != bytes.Length - 1)
        //        {
        //            macAddress += "-";
        //        }
        //    }
        //    US.MacAddress = macAddress;
        //}

        public static void SetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            List<string> macList = new List<string>();
            foreach (var nic in nics)
            {
                string macAddress = string.Empty;
                PhysicalAddress address = nic.GetPhysicalAddress();
                byte[] bytes = address.GetAddressBytes();
                for (int i = 0; i < bytes.Length; i++)
                {
                    macAddress = macAddress + string.Format("{0}", bytes[i].ToString("X2"));
                    if (i != bytes.Length - 1)
                    {
                        macAddress += "-";
                    }
                }
                if (Array.IndexOf(nics, nic) == 0)
                    US.MacAddress = macAddress;
                macList.Add(macAddress);
            }
            US.MacAddressList = macList.ToArray();
        }
        public static bool ValidFiscalYear = false;

        public static bool DashboardView = false;

        public static bool ShowLogoOnTicket = true;

        public static float TicketSize = 0;

        public static DateTime SelectedDateTime = DateTime.Now;
        public static BookinVerifyDetail BookinVerifyDetails { get; set; } = null;
        public static int DashboardLocationID { get; set; } = 0;
        public static bool BackToDashboardFromTicketDetails = false;
    }
    public class BookinVerifyDetail
    {
        public string BookingNumber { get; set; }
        public bool IsBookingChecked { get; set; }
    }
}