namespace TicketApp.Helper
{
    public class PrintDetail
    {
        public static string CompanyName = "Event Cinemas";
        public static string Address = "Lalbandi ";
        public static string SerialNumber = "ART - 0101 ";
        public static string TheaterVatNo = "vat - 0101 ";
        public static string Screen = "audi 1";
        public static string VatNo = "619190590";
        
        public string FiscalID { get; set; }
        public static string TicketType = "Special";
        public static string SeatNo = "A10";
        public static float TotalCost = 400;
        public string BarCode { get; set; }
        public static string ShowTime = "11:00";
        public static string Movie = "Pathan";
        public static bool IsInterNational = true;
        public int UserTicketID { get; set; }
        public int RefreshmentTicketID { get; set; }
        public string RollQuality { get; set; }
        public DateTime ShowDate { get; set; }
        public string ShoDateOnly
        {
            get
            {
                return ShowDate.ToString("M/d/yyyy");
            }
        }
        public int StatusID { get; set; }
        public float CancellationCharge { get; set; }
        public float DiscountAmount { get; set; }
        public float ConvinienceCharge { get; set; }
        public float orderServiceTAX { get; set; }
        public float OrderVAT { get; set; }
        public int ProductID { get; set; }
        public string MenuName { get; set; }
        public float MenuPrice { get; set; }
        public int MenuQuentity { get; set; }
        public float MenuTotalPrice { get; set; }
        public float ItemDiscount { get; set; }
        public bool ItemIsDiscountable { get; set; }
        public float ItemCancellationCharge { get; set; }
        public float ItemMembershipDiscount { get; set; }
        public bool ItemIsServiceCharge { get; set; }
        public decimal CoverCharge { get; set; }
        public int OrderID { get; set; }
        public decimal TotalAmount { get; set; }
        public string RefMessage { get; set; }
        public string RefTitle { get; set; }
        public string BoxOfficeTaxRate { get; set; }
        public string LocalTaxRate { get; set; }
        public bool IsComplimentary { get; set; }
        public string ThreeDSerialNumber { get; set; }
        public bool IsOriginalCopy { get; set; }
        public int OriginalCopyCount { get; set; }

    }
}
