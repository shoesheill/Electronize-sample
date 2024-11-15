namespace TicketApp.Helper
{
    public class TicketDetails
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Time  { get; set; }
        public int Price { get; set; }
        public string IsThreeD { get; set; }
        public string Screen { get; set; }
        public string TicketType { get; set; }
        public string ShowDate { get; set; }
        public string IsInternational { get; set; }
        public string TokenTitle { get { return Title + " - " +Convert.ToDateTime(ShowDate).ToString("MMM-dd-yyyy") + " - " + Convert.ToDateTime(Time).ToString("hh:mm tt") + " ( " + TicketType + " )"; } }

    }
}
