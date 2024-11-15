using System.Drawing;

namespace PosPrinterApp.Helper
{
    class US
    {
        public const string ConfigFileName = "Cineplex.xer";
        public const int LINETHICKNESS = 1;
        public static string MacAddress = "";
        public static string[] MacAddressList = new string[] { };

        public const string APPGUID = "{9F6F0AC4-B9A1-45fd-A8CF-72F04T6BDE8S}";

        public static string[] ALPHABETS = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };


        public const string SeatFont = "Segoe UI Semibold";

        public const string APPNAME = "SageFlick";
        public const string LOGINHEADER = "Welcome to the login page";

        public const string FONT = "Calibri";
        public static Font DefaultFont = new Font("Calibri", 12, FontStyle.Regular);

        public const string ButtonText = "Button";
        public const string TextBoxDescription = "Sets the Text";
        public const string PropCategory = "Cine";

        public const string DateFormat2 = "yyyy/MM/dd dddd";
        public const string DateFormat3 = "dddd";

        public const string PerSemi = "%):";
        public const string BraceOpen = "(";
        public const string BraceClose = ")";
        public const string Astrisk = " * ";
        public const string DateFormat = "yyyy/MM/dd";
        public const string TimeFormat = "hh:mm tt";
        public const string DefaultImagePath = "Assets/movie.png";
        public const string DefaultImagePath2 = "Assets/package.png";
        public const string SF_OpointOO = "0.00";
        public const string RsFormat = " Rs. {0:00}";
        public const string ReturnFormat = "{0:00}";
        public const string RsFormat2 = "Rs. {0} ";
        public const string RsFormat3 = "Rs. ";
        public const string RsFormat4 = "";
        public const string RsFormat5 = "Rs. 0.00";
        public const string Zero = "0";
        public const string Colon = ": ";
        public const string NetTotal = "Net Total: Rs. {0}";
        public const string NetTotal2 = "Net Total: Rs.";
        public const string ReturnRs2 = "Return: Rs.";
        public const string GrossTotal = "Gross Total : Rs {0}";
        public const string ReturnRs = "Return: Rs. {0}";
        public const string Package = "package";
        public const string Packages = "Packages";
        public const string PackageHeader = "PACKAGES";
        public const string MenuItem = "menuitem";
        public const string TicketMiniCartHeader = "Ticket Mini Cart";
        public const string PreviousOrderHeader = "Previous Order";

        public const string PreDateLabel = "Selected Date: ";
        public const string TaxInvoice = "Tax Invoice";
        public const string Abbreviated = "Abbreviated ";
        public const string ServiceCharge = "Service Charge ";
        public const string Vat = "VAT ";
        public const string VatNo = "Vat no. ";
        public const string PreFoodItems = "Food Items ({0})";
        public const string PreTotal = "Total: Rs. {0}";
        public const string PreSoldSeatLabel = "Sold Seat: {0}";
        public const string PreBookedSeatLabel = "Booked Seat: {0}";
        public const string PreComplementaryLabel = "Complimentary: {0}";
        public const string PreBookingHoldLabel = "Booking Hold Seat: {0}";
        public const string PreQRSoldSeatLabel = "Sold Seat(QR Pay): {0}";
        public const string PreElectronicCardLabel = "Sold Seat(Electronic Card): {0}";
        public const string PreRemainingLabel = "Remaining: {0}";


        public const string PreSeat = "Seat#: {0}";
        public const string PreRefreshmentChargeLabel = "Refreshment Charge = Rs. {0}";
        public const string PreTicketTotalLabel = "Ticket Total:  Rs. {0:00}";
        public const string PreConvinienceVatRate = "Convinience VAT ({0}%):  Rs. {1}";
        public const string PreConvinienceCharge = "Convinience Charge:  Rs. ";
        public const string PreRemainingBalance = "Your remaining balance is: Rs. {0:00}";
        public const string PreAvailableBalance = "Your available balance is: Rs. {0:00}";
        public const string PreFoodSubTotalLabel = "Food Sub Total : ";
        public const string PreFoodTotalLabel = "Food Total : ";
        public const string PreGrandTotalLabel = "Grand Total : ";
        public const string MainPageTitle = "MainPage";
        public const string Pre_TopUP = "top-up of Rs. ";
        public const string Post_Successfull = " is successful";
        public const string Pre_Name_Remaining_Balance = "Name: {0}\nRemaining balance: Rs. {1}";
        public const string Post_Cash_To_Be_Refunded = "\nCash to be refunded: Rs.";
        public const string Pre_Cancellation_Process_Complete = "The cancellation process is complete.\nCash to be refunded: Rs.";

        public const string PaymentTypeValueMember = "PaymentTypeID";
        public const string PaymentTypeDisplayMember = "PaymentTypeTitle";

        public const string ShowTimeInfoDisplayMember = "StartTimes";
        public const string ScreenInfoDisplayMember = "Title";
        public const string ScreenInfoValueMember = "ScreenID";


        public const string ViewOrder = "View Order";
        public const string HideOrder = "Hide Order";

        public const string ScreenByTickectTypeValueMember = "TicketTypeID";
        public const string ScreenByTickectTypeDisplayMember = "Title";

        public const string SeatViewModeValueMember = "SeatViewModeID";
        public const string SeatViewModeDisplayMember = "ViewMode";
        //public const string Complete = "Complete";
        public const string DashboardInPrimary = "DashboardInPrimary";


    }
}
