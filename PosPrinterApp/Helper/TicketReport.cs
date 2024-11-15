namespace PosPrinterApp.Helper
{
    public class TicketReport
    {
        public string Movie { get; set; }
        public string ShowDate { get; set; }
        public string ShowTime { get; set; }
        public string TicketType { get; set; }
        public float Price { get; set; }
        public float Net { get; set; }
        public float Vat { get; set; }
        public float LocalTax { get; set; }
        public float FDF { get; set; }

    }
}
