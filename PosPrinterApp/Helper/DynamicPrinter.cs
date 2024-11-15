using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using TicketApp;
using TicketApp.Helper;

namespace PosPrinterApp.Helper
{
    public enum PrintStatus { Ready = 0, Processing = 1, Completed = 2, Error = 3 };
    public enum PrintPaperType { Ticket };

    public enum PrintType { Preview, Dialog, Default };
    public class DynamicTicketPrintInfo
    {
        public object DynamicInfo { get; set; }
    }
    public class DynamicPrinterStatusHandler
    {
        public event EventHandler<DynamicPrinterEventArgs> PrinterStatusChanged;

        internal void OnPrinterStatusChanged(string message, PrintStatus status)
        {
            EventHandler<DynamicPrinterEventArgs> temp = PrinterStatusChanged;
            if (temp != null)
            {
                DynamicPrinterEventArgs e = new DynamicPrinterEventArgs();
                e.Message = message;
                e.Status = status;
                PrinterStatusChanged(this, e);
            }
        }
    }
    public class DynamicPrinterEventArgs : EventArgs
    {
        public string Message;
        public PrintStatus Status = 0;
    }
    public class DynamicPrinter : DynamicPrinterStatusHandler
    {
        private Color _defaultColor = Color.Black;

        //private Font _font = new Font("Arial", 10);

        private object dynamicParam = null;

        //   public event DynamicPrinterEventArgs PrinterStatusChanged=null;
        private int leftMargin = 10;

        private float pageWidth = 80;

        private PrintDocument printDOC = null;

        private PageSettings printerPageSetting = null;

        private int rightMargin = 10;

        /// <summary>
        /// Default Constructor
        /// </summary>
        //public DynamicPrinter()
        //{
        //    PrintDOC = new PrintDocument();
        //    PrinterPageSetting = PrintDOC.DefaultPageSettings;
        //    PageWidth = PrinterPageSetting.PrintableArea.Width;
        //    PrintDOC.PrintPage += Test_PrintPage;
        //}

        // public DynamicPrinter()
        // {
        //     PrintDOC = new PrintDocument();
        //     PrinterPageSetting = PrintDOC.DefaultPageSettings;
        //     PageWidth = PrinterPageSetting.PrintableArea.Width;
        //     PrintDOC.DefaultPageSettings.Margins.Top = 0;
        //     PrintDOC.PrinterSettings.DefaultPageSettings.Margins.Top = 0;
        //     if (DataHolder.TicketSize > 0)
        //     {
        //         int w = (int)PageWidth;
        //
        //         int defaultMargin = 15;
        //
        //         int ticketHeight = (int)((DataHolder.TicketSize - defaultMargin) * 3.938356);
        //
        //         PaperSize p = new PaperSize("ticketSize", w, ticketHeight);
        //         PrintDOC.DefaultPageSettings.PaperSize = p;
        //         PrintDOC.PrinterSettings.DefaultPageSettings.PaperSize = p;
        //         PrintDOC.DefaultPageSettings.Margins.Top = 0;
        //         PrintDOC.PrinterSettings.DefaultPageSettings.Margins.Top = 0;
        //     }
        //
        //     if (StaticData.IsCCMS)
        //         PrintDOC.PrintPage += new PrintPageEventHandler(_printDoc_PrintPage);
        //     else
        //         PrintDOC.PrintPage += new PrintPageEventHandler(Ticket_NewPrintPage);
        //
        //
        // }

        public Color DefaultColor
        { get { return _defaultColor; } set { _defaultColor = value; } }

        public object DynamicParam
        { get { return dynamicParam; } set { dynamicParam = value; } }

        public int LeftMargin
        { get { return leftMargin; } set { leftMargin = value; } }

        public float PageWidth
        { get { return pageWidth; } set { pageWidth = value; } }

        public PrintDocument PrintDOC
        { get { return printDOC; } set { printDOC = value; } }

        public PageSettings PrinterPageSetting
        { get { return printerPageSetting; } set { printerPageSetting = value; } }

        // public Font PrintFont
        // { get { return _font; } set { _font = value; } }

        public int RightMargin
        { get { return rightMargin; } set { rightMargin = value; } }

        /*-----------[Print Document]---------------*/
        /*-----------[DEFAULT TEXT COLOR]---------------*/
        /*-----------[FONT]---------------*/
        /*-----------[Printer Page Setting]---------------*/
        /*-----------[Printing Page Width]---------------*/
        /*-----------[Object Info for custom printing]---------------*/
        // public void Print(PrintType objPrintType)
        // {
        //     try
        //     {
        //         switch (objPrintType)
        //         {
        //             case PrintType.Default:
        //                 PrintDOC.Print();
        //                 break;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         OnPrinterStatusChanged(ex.Message, PrintStatus.Error);
        //     }
        // }
        protected Action<Graphics> print;
        protected float width;

        public void Print()
        {
            this.print = print;

            var pd = new PrintDocument();

            //pd.PrinterSettings.PrinterName = printerName; // TODO: it would be better to take printer name from the configuration settings
            pd.PrinterSettings.PrinterName = new PrinterSettings().PrinterName;
        
        
           // pd.PrintPage += PrintPage;
           
           if (StaticData.IsCCMS)
               pd.PrintPage += new PrintPageEventHandler(_printDoc_PrintPage);
               else
                   pd.PrintPage += new PrintPageEventHandler(Ticket_NewPrintPage);
            width = pd.DefaultPageSettings.PrintableArea.Width * 2.539993f / 10f; // Convert inches to millimeters

            try
            {
                pd.Print();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


            private SizeF GetStringSize(Graphics g, string text, Font font)
        { return g.MeasureString(text.ToString(), font); }
        public void _printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            TicketDetails ticket = StaticData.lstTicket.Where(c => c.ID == StaticData.ID).First();
            bool ApplyVatInComplementry = false;

            bool ApplyFDFInComplementry = false;
            bool ApplyLocalTaxInComplementry = false;
            bool EnableTaxRegistration = true;
            bool ShowLogoOnTicket = true;

            float TotalCost = 0;
            float EntranceFee = 0;
            float FDF = 0;
            float VAT = 0;
            float LocalTax = 0;
            float ThreeDCharge = 0;
            float ConvinienceCharge = 0;
            float discount = 0;

            float tempTotal = 1000;
            TotalCost = 0;
            EntranceFee = 0;
            FDF = 0;
            VAT = 0;
            LocalTax = 0;
            ThreeDCharge = 0;
            ConvinienceCharge = 0;
            discount = 0;

            discount = 0;
            bool _isRePrint = false;


            if (ticket.IsInternational.Equals("1"))
            {
                EntranceFee = ticket.Price / ((1 + StaticData.BoxOfficeTax + StaticData.LocalTax) * (1 + StaticData.EntertainmentTax));
                FDF = EntranceFee * StaticData.BoxOfficeTax;
            }
            else
            {
                EntranceFee = ticket.Price / ((1 + StaticData.EntertainmentTax) * (1 + StaticData.LocalTax));
            }
            LocalTax = EntranceFee * StaticData.LocalTax;
            VAT = (EntranceFee + FDF + LocalTax) * StaticData.EntertainmentTax;
            ConvinienceCharge = StaticData.ConvinienceCharge;
            ThreeDCharge = StaticData.ThreeDCharge;
            TotalCost = ticket.Price + ConvinienceCharge + ThreeDCharge;

            if (StaticData.IsComplimentary)
            {
                EntranceFee = 0;
                ConvinienceCharge = 0;
                ThreeDCharge = 0;
                TotalCost = 0;
                if (!ApplyFDFInComplementry)
                    FDF = 0;
                if (!ApplyLocalTaxInComplementry)
                    LocalTax = 0;
                if (!ApplyVatInComplementry)
                    VAT = 0;
            }
            //WriteToExcel(ticket, EntranceFee, VAT, FDF, LocalTax); ;
            using (Brush brush = new SolidBrush(Color.Black))
            {
                using (Font HEAD_FOCUS = new Font("Segoe UI", 12, FontStyle.Bold))
                {
                    using (Font HEAD = new Font("Segoe UI", 8, FontStyle.Bold))
                    {
                        using (Font HEAD_BOLD = new Font("Segoe UI", 10, FontStyle.Bold))
                        {
                            using (Font HEAD_SMALL = new Font("Segoe UI", 8, FontStyle.Bold))
                            {
                                using (Font LABEL = new Font("Segoe UI", 8, FontStyle.Regular))
                                {
                                    using (Font LABEL_BOLD = new Font("Segoe UI", 8, FontStyle.Bold))
                                    {
                                        using (Font LABEL_SMALL = new Font("Segoe UI", 8, FontStyle.Regular))
                                        {

                                            //if (isSample > 0)
                                            //{
                                            //    Image sample = Resources.sample;//Utilities.ResizeImage(Resources.sample, new Size(350, 50));
                                            //    e.Graphics.DrawImage(sample, X_Image_Center_Point(sample), 100);
                                            //}

                                            float x = 0;
                                            float y = 0;
                                            SizeF printTextSize = new SizeF();
                                            float lineSpace = 0;
                                            float leftMargin = 5;
                                            string printText = string.Empty;
                                            string printVText = string.Empty;

                                            using (Image Logo = Utilities.ResizeImage(Image.FromFile(StaticData.LogoPath), new Size(160, 50)))
                                            {
                                                x = X_Image_Center_Point(Logo);
                                                e.Graphics.DrawImage(Logo, x, y);
                                                y += Logo.Height + 5;
                                            }
                                            printText = PrintDetail.CompanyName.Trim();
                                            printTextSize = StringSize(e.Graphics, printText, HEAD_BOLD);
                                            x = X_Text_Center_Point(printTextSize);
                                            e.Graphics.DrawString(printText, HEAD_BOLD, brush, x, y);
                                            lineSpace = printTextSize.Height;
                                            y += lineSpace - 5;

                                            printText = PrintDetail.Address.Trim();
                                            printTextSize = StringSize(e.Graphics, printText, HEAD_SMALL);
                                            x = X_Text_Center_Point(printTextSize);
                                            e.Graphics.DrawString(printText, HEAD_SMALL, brush, x, y);
                                            lineSpace = printTextSize.Height;
                                            y += lineSpace + 1;

                                            if (_isRePrint)
                                            {
                                                printText = "Invoice";
                                            }
                                            else
                                            {
                                                printText = "Tax Invoice";
                                            }
                                            printTextSize = StringSize(e.Graphics, printText, HEAD);
                                            x = X_Text_Center_Point(printTextSize);
                                            e.Graphics.DrawString(printText, HEAD, brush, x, y);
                                            lineSpace = printTextSize.Height;
                                            y += lineSpace + 1;

                                            #region Invoice No
                                            printText = "Invoice No:";
                                            x = leftMargin;
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            x += printTextSize.Width;
                                            Random generator = new Random();
                                            printText = "BUD-" + generator.Next(1, 99).ToString("D2") + "-" + generator.Next(0, 99999).ToString("D9");
                                            x += 1;
                                            e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);
                                            //lineSpace = printTextSize.Height;
                                            y += lineSpace;
                                            #endregion

                                            #region Vat No
                                            //y += 5;
                                            printText = "VAT No. :";
                                            x = leftMargin;
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            x += printTextSize.Width;

                                            //y -= 5;
                                            printText = PrintDetail.VatNo.Trim();
                                            x += 1;
                                            printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);
                                            lineSpace = printTextSize.Height - 10;
                                            //y += lineSpace;
                                            #endregion

                                            #region TicketBatchNumber
                                            printText = generator.Next(1, 99).ToString("D4");
                                            printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);

                                            printText = "TicketBatch No. :";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            x -= (printTextSize.Width);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            y += lineSpace;
                                            #endregion

                                            #region Movie
                                            y += 5;
                                            printText = "Movie : ";
                                            x = leftMargin;
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            x += printTextSize.Width;

                                            y -= 5;
                                            printText = ticket.Title.Trim() + (ticket.IsThreeD.Equals("1") ? " (3D)" : " (2D)");
                                            x += 1;
                                            printTextSize = StringSize(e.Graphics, printText, HEAD_FOCUS);
                                            e.Graphics.DrawString(printText, HEAD_FOCUS, brush, x, y);
                                            lineSpace = printTextSize.Height - 10;
                                            y += lineSpace;
                                            #endregion

                                            #region Screen
                                            y += 5;
                                            printText = "Screen : ";
                                            x = leftMargin;
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            x += printTextSize.Width;

                                            y -= 5;
                                            printText = ticket.Screen.Trim();
                                            x += 1;
                                            printTextSize = StringSize(e.Graphics, printText, HEAD_FOCUS);
                                            e.Graphics.DrawString(printText, HEAD_FOCUS, brush, x, y);
                                            lineSpace = printTextSize.Height - 10;
                                            #endregion

                                            #region TicketType
                                            printText = ticket.TicketType.Trim();
                                            printTextSize = StringSize(e.Graphics, printText, HEAD_FOCUS);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, HEAD_FOCUS, brush, x, y);

                                            y += 5;
                                            printText = "Ticket Type : ";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            x -= (printTextSize.Width);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            y += lineSpace;
                                            #endregion

                                            //#region ShowType
                                            //printText = "Show Type : ";
                                            //x = leftMargin;
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //x += printTextSize.Width;

                                            //printText = _objPrint.ShowType.Trim();
                                            //x += 1;
                                            //e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);
                                            ////y += lineSpace; // remove this if category shown
                                            //#endregion

                                            //#region Category
                                            //printText = _objPrint.ShowCategory.Trim();
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            //x = X_Text_Right_Point(printTextSize);
                                            //e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);

                                            //printText = "Show Category : ";
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //x -= (printTextSize.Width);
                                            //e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //y += lineSpace;
                                            //#endregion

                                            //#region CustomerName
                                            //printText = "Cust. Name: ";
                                            //x = leftMargin;
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //x += printTextSize.Width;

                                            //printText = _objPrint.CustomerName.Trim();
                                            //x += 1;
                                            //e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);
                                            //#endregion

                                            //#region CustomerPan No
                                            //printText = _objPrint.CustomerPAN.Trim();
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            //x = X_Text_Right_Point(printTextSize);
                                            //e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);

                                            //printText = "PAN No. : ";
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //x -= (printTextSize.Width);
                                            //e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //y += lineSpace;
                                            //#endregion

                                            #region Date
                                            printText = "Date: ";
                                            x = leftMargin;
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            x += printTextSize.Width;

                                            printText = Convert.ToDateTime(ticket.ShowDate).ToString("MMM-dd-yyyy");
                                            x += 1;
                                            e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);
                                            #endregion

                                            #region Time
                                            printText = Convert.ToDateTime(ticket.Time).ToString("hh:mm tt");
                                            printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);

                                            printText = "Time: ";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            x -= (printTextSize.Width);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            y += lineSpace;
                                            #endregion

                                            #region Seat/Quantity/Amount

                                            // Labels
                                            printText = "Seat No.";
                                            x = leftMargin;
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);

                                            //printText = "Qty";
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //x = X_Text_Center_Point(printTextSize);
                                            //e.Graphics.DrawString(printText, LABEL, brush, x, y);

                                            printText = "Quantity";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            x = X_Text_Center_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);

                                            printText = "Per Unit";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            y += lineSpace;
                                            // End Labels

                                            // Values
                                            string a = new string(Enumerable.Range(0, 1).Select(_ => (char)generator.Next('A', 'N')).ToArray());
                                            generator.Next(1, 20).ToString("D2");
                                            printText = a + generator.Next(1, 20).ToString("D2");
                                            x = leftMargin;
                                            printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);

                                            //printText = "1";
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            //x = X_Text_Center_Point(printTextSize);
                                            //e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);

                                            printText = "1";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            x = X_Text_Center_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);


                                            printText = EntranceFee.ToString("0.00");
                                            printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);
                                            y += lineSpace;


                                            //printText = EntranceFee.ToString("0.00");
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            //x = X_Text_Right_Point(printTextSize);
                                            //e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);

                                            // y += 5;
                                            printText = "Total : ";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            x -= (printTextSize.Width);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            printText = EntranceFee.ToString("0.00");
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);
                                            y += lineSpace;


                                            printText = "Taxable Amount: Rs. ";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            x -= (printTextSize.Width);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            printText = EntranceFee.ToString("0.00");
                                            printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            y += lineSpace;
                                            if (FDF > 0)
                                            {
                                                printText = "FDF(" + (StaticData.BoxOfficeTax * 100).ToString("0") + "%): ";
                                                printVText = "Rs. " + FDF.ToString("0.00");
                                                printTextSize = StringSize(e.Graphics, (printText + printVText), LABEL);
                                                x = X_Text_Right_Point(printTextSize);
                                                e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                                printTextSize = StringSize(e.Graphics, printVText, LABEL);
                                                x = X_Text_Right_Point(printTextSize);
                                                e.Graphics.DrawString(printVText, LABEL, brush, x, y);
                                                y += lineSpace;
                                            }
                                            printText = "SLET(" + (StaticData.LocalTax * 100).ToString("0") + "%): ";
                                            printVText = "Rs. " + LocalTax.ToString("0.00");
                                            printTextSize = StringSize(e.Graphics, (printText + printVText), LABEL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            printTextSize = StringSize(e.Graphics, printVText, LABEL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printVText, LABEL, brush, x, y);
                                            y += lineSpace;

                                            printText = "VAT(" + (StaticData.EntertainmentTax * 100).ToString("0") + "%): ";
                                            printVText = "Rs. " + VAT.ToString("0.00");
                                            printTextSize = StringSize(e.Graphics, (printText + printVText), LABEL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            printTextSize = StringSize(e.Graphics, printVText, LABEL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printVText, LABEL, brush, x, y);
                                            y += lineSpace;


                                            printText = "Total Cost: ";
                                            printVText = "Rs. " + ticket.Price.ToString("0.00");
                                            printTextSize = StringSize(e.Graphics, (printText + printVText), LABEL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            printTextSize = StringSize(e.Graphics, printVText, LABEL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printVText, LABEL_BOLD, brush, x, y);
                                            y += lineSpace;




                                            //printText = "FDF";
                                            //printText += "(" + StaticData.BoxOfficeTax.ToString("0.00") + "%)";
                                            //printText += " : ";
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //x -= (printTextSize.Width);
                                            //e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //y += lineSpace;

                                            //printText = "Rs. " + FDF.ToString("0.00");
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //x = X_Text_Right_Point(printTextSize);
                                            //e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //decimal TotalCost;
                                            //foreach (CalculatedTax tax in _objPrint.IncludedTax)
                                            //{
                                            //    if (tax.Title == "Entrance Fee")
                                            //    {
                                            //        printText = tax.Value.ToString("0.00");
                                            //        printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            //        x = X_Text_Right_Point(printTextSize);
                                            //        e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);
                                            //        y += lineSpace;
                                            //        // End Values
                                            //        #endregion

                                            //        //printText = "Total";
                                            //        //printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //        //x = X_Text_Right_Point(printTextSize);
                                            //        //e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //        //y += lineSpace;

                                            //        //printText = tax.Value.ToString("0.00");
                                            //        //printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            //        //x = X_Text_Right_Point(printTextSize);
                                            //        //e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);
                                            //        //y += lineSpace;

                                            //        printText = tax.Value.ToString("0.00");
                                            //        printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            //        x = X_Text_Right_Point(printTextSize);
                                            //        e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);

                                            //        // y += 5;
                                            //        printText = "Total : ";
                                            //        printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //        x -= (printTextSize.Width);
                                            //        e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //        y += lineSpace;
                                            //    }
                                            //}

                                            //decimal TotalCost;
                                            //TotalCost = _objPrint.PaymentTypeID == 2 ? 0 : _objPrint.Price;
                                            //int count = 1;
                                            //foreach (CalculatedTax tax in _objPrint.IncludedTax)
                                            //{
                                            //    if (count == 1)
                                            //    {
                                            //        if (_objPrint.ScreenLogo != null & !string.IsNullOrEmpty(_objPrint.ScreenLogo))
                                            //        {
                                            //            Image _img = Utilities.ResizeImage(StaticItems.Base64ToImage(_objPrint.ScreenLogo), new Size(70, 30));
                                            //            x = leftMargin;
                                            //            e.Graphics.DrawImage(_img, x, y);
                                            //        }
                                            //    }

                                            //    printText = "Rs. " + tax.Value.ToString("0.00");
                                            //    printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //    x = X_Text_Right_Point(printTextSize);
                                            //    e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //    if (tax.Title == "Entrance Fee")
                                            //    {
                                            //        printText = "Taxable Amount";
                                            //        printText += tax.Unit == 1 ? "(" + tax.Rate.ToString("0.00") + "%)" : "";
                                            //        printText += " : ";
                                            //        printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //        x -= (printTextSize.Width);
                                            //        e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //        y += lineSpace;
                                            //    }
                                            //    else
                                            //    {
                                            //        printText = tax.Title;
                                            //        printText += tax.Unit == 1 ? "(" + tax.Rate.ToString("0.00") + "%)" : "";
                                            //        printText += " : ";
                                            //        printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //        x -= (printTextSize.Width);
                                            //        e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //        y += lineSpace;
                                            //    }
                                            //    if (count == 1)
                                            //    {
                                            //        if (_objPrint.TicketTypeLogo != null & !string.IsNullOrEmpty(_objPrint.TicketTypeLogo))
                                            //        {
                                            //            Image _image = Utilities.ResizeImage(StaticItems.Base64ToImage(_objPrint.TicketTypeLogo), new Size(70, 30));
                                            //            x = leftMargin;
                                            //            e.Graphics.DrawImage(_image, x, y);
                                            //        }
                                            //    }
                                            //    count++;
                                            //}

                                            //foreach (CalculatedCharge charge in _objPrint.IncludedCharge)
                                            //{
                                            //    printText = "Rs. " + charge.NetValue.ToString("0.00");
                                            //    printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //    x = X_Text_Right_Point(printTextSize);
                                            //    e.Graphics.DrawString(printText, LABEL, brush, x, y);

                                            //    printText = charge.Title;
                                            //    //printText += charge.Unit == 1 ? "(" + charge.Rate.ToString("0.00") + "%)" : "";
                                            //    printText += " : ";
                                            //    printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //    x -= (printTextSize.Width);
                                            //    e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //    y += lineSpace;

                                            //    printText = "Rs. " + charge.VatValue.ToString("0.00");
                                            //    printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //    x = X_Text_Right_Point(printTextSize);
                                            //    e.Graphics.DrawString(printText, LABEL, brush, x, y);

                                            //    printText = charge.Title + "(VAT) : ";
                                            //    printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //    x -= (printTextSize.Width);
                                            //    e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //    y += lineSpace;

                                            //    TotalCost += charge.Unit == 2 ? charge.Rate : (charge.Rate * _objPrint.Price * (decimal)0.01);
                                            //}

                                            //printText = "Rs. " + TotalCost.ToString("0.00");
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL_BOLD);
                                            //x = X_Text_Right_Point(printTextSize);
                                            //e.Graphics.DrawString(printText, LABEL_BOLD, brush, x, y);

                                            //printText = "Total Cost : ";
                                            //printTextSize = StringSize(e.Graphics, printText, LABEL);
                                            //x -= (printTextSize.Width);
                                            //e.Graphics.DrawString(printText, LABEL, brush, x, y);
                                            //y += lineSpace;

                                            //if (_objPrint.PaymentTypeID == 2)
                                            //{
                                            //    y += 2;
                                            //    printText = "COMPLIMENTARY";
                                            //    printTextSize = StringSize(e.Graphics, printText, HEAD);
                                            //    x = X_Text_Center_Point(printTextSize);
                                            //    e.Graphics.DrawString(printText, HEAD, brush, x, y);
                                            //    y += lineSpace;
                                            //}
                                            //Random generator = new Random();
                                            String r = generator.Next(0, 999999).ToString("D16");
                                            Image barCodeImg = Utilities.GenerateBarCode(r.Trim(), 200, 70);
                                            x = X_Image_Center_Point(barCodeImg);
                                            e.Graphics.DrawImage(barCodeImg, x, y);
                                            y += barCodeImg.Height + 15;

                                            //if (_isRePrint)
                                            //{
                                            //    printText = string.Format("{0}-{1}", "Copy of Original", _objPrint.RePrintCount);
                                            //    printTextSize = StringSize(e.Graphics, printText, HEAD_BOLD);
                                            //    x = X_Text_Center_Point(printTextSize);
                                            //    e.Graphics.DrawString(printText, HEAD_BOLD, brush, x, y);
                                            //    y += printTextSize.Height;
                                            //}

                                            printText = "Printed On : ";
                                            x = leftMargin;
                                            printTextSize = StringSize(e.Graphics, printText, LABEL_SMALL);
                                            e.Graphics.DrawString(printText, LABEL_SMALL, brush, x, y);
                                            x += printTextSize.Width;

                                            printText = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                                            e.Graphics.DrawString(printText, LABEL_SMALL, brush, x, y);

                                            //printText = _objPrint.Username.Trim();
                                            printText = "admin";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL_SMALL);
                                            x = X_Text_Right_Point(printTextSize);
                                            e.Graphics.DrawString(printText, LABEL_SMALL, brush, x, y);

                                            printText = "Printed By : ";
                                            printTextSize = StringSize(e.Graphics, printText, LABEL_SMALL);
                                            x -= (printTextSize.Width);
                                            e.Graphics.DrawString(printText, LABEL_SMALL, brush, x, y);
                                            y += lineSpace;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private float X_Image_Center_Point(Image img)
        {
            float x = (printDOC.DefaultPageSettings.PrintableArea.Width - img.Width) / 2;
            return x;
        }

        private float X_Text_Center_Point(SizeF stringSize)
        {
            float x = (printDOC.DefaultPageSettings.PrintableArea.Width - stringSize.Width) / 2;
            return x;
        }

        private float X_Text_Right_Point(SizeF stringSize)
        {
            float x = (printDOC.DefaultPageSettings.PrintableArea.Width - stringSize.Width - 10);
            return x;
        }

        private SizeF StringSize(Graphics g, string text, Font font)
        {
            return g.MeasureString(text, font);
        }


        private void Ticket_NewPrintPage(object sender, PrintPageEventArgs e)
        {
            //StaticData sd=new StaticData();
            TicketDetails ticket = StaticData.lstTicket.Where(c => c.ID == StaticData.ID).First();

            string printType = "qrcode";
            bool ApplyVatInComplementry = false;

            bool ApplyFDFInComplementry = false;
            bool ApplyLocalTaxInComplementry = false;
            bool EnableTaxRegistration = true;
            bool ShowLogoOnTicket = true;

            float TotalCost = 0;
            float EntranceFee = 0;
            float FDF = 0;
            float VAT = 0;
            float LocalTax = 0;
            float ThreeDCharge = 0;
            float ConvinienceCharge = 0;
            float discount = 0;

            float tempTotal = 1000;
            TotalCost = 0;
            EntranceFee = 0;
            FDF = 0;
            VAT = 0;
            LocalTax = 0;
            ThreeDCharge = 0;
            ConvinienceCharge = 0;
            discount = 0;

            discount = 0;



            if (ticket.IsInternational.Equals("1"))
            {
                EntranceFee = tempTotal / ((1 + StaticData.BoxOfficeTax + StaticData.LocalTax) * (1 + StaticData.EntertainmentTax));
                FDF = EntranceFee * StaticData.BoxOfficeTax;
            }
            else
            {
                EntranceFee = tempTotal / ((1 + StaticData.EntertainmentTax) * (1 + StaticData.LocalTax));
            }
            LocalTax = EntranceFee * StaticData.LocalTax;
            VAT = (EntranceFee + FDF + LocalTax) * StaticData.EntertainmentTax;
            ConvinienceCharge = StaticData.ConvinienceCharge;
            ThreeDCharge = StaticData.ThreeDCharge;
            TotalCost = ticket.Price + ConvinienceCharge + ThreeDCharge;

            if (StaticData.IsComplimentary)
            {
                EntranceFee = 0;
                ConvinienceCharge = 0;
                ThreeDCharge = 0;
                TotalCost = 0;
                if (!ApplyFDFInComplementry)
                    FDF = 0;
                if (!ApplyLocalTaxInComplementry)
                    LocalTax = 0;
                if (!ApplyVatInComplementry)
                    VAT = 0;
            }


            Brush brush = new SolidBrush(DefaultColor);
            string sFont = US.FONT;
            Font semiLightFont = new Font(sFont, 6F);
            Font semiLightBoldFont = new Font(sFont, 6.5F, FontStyle.Bold);
            Font semiLightBoldFont5F = new Font(sFont, 5.5F, FontStyle.Bold);
            Font LightFont = new Font(sFont, 8F);
            Font SEMIBOLD_FONT = new Font(sFont, 7.5F, FontStyle.Bold);
            Font BOLD_FONT = new Font(sFont, 9.5F, FontStyle.Bold);
            Font LARGE_BOLD_FONT = new Font(sFont, 13F, FontStyle.Bold);

            float x = 0;
            float y = 0;

            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            string appPath = dirPath;
            ImageHelper helper = new ImageHelper();
            string barcode = "12345678912";

            //DirectoryInfo di = new DirectoryInfo(appPath + @"\CineUploadFiles\TicketCode\");
            //if (di.Exists) di.Delete(true);

            Image img = helper.GenerateImageBitMap(barcode, appPath + @"\CineUploadFiles\TicketCode\");


            //LOGO
            Image logo = Utilities.GetPrintLogo();
            //logo = Utilities.ResizeImage(logo, new Size(180, 80), true);
            int logoHeight = logo.Height;
            if (ShowLogoOnTicket)
            {
                //    y = 0;
                //    x = X_Point_Center(e.Graphics, logo);
                //    e.Graphics.DrawImage(logo, x, y);
                //    y += logo.Height - 5;

                // Image logo = objInfo.LogoImage;
                x = X_Point_Center(e.Graphics, logo);
                e.Graphics.DrawImage(logo, x, 0);
                y += logo.Height - 10;
            }
            else
            {
                y += logo.Height / 3 + 0;
            }

            string printingText = string.Empty;
            float lineSpacing = 0;
            SizeF size = new SizeF();

            if (PrintDetail.CompanyName != "" && PrintDetail.CompanyName != null)
            {
                //HALL Company Name
                printingText = PrintDetail.CompanyName.Trim();
                x = X_Point_Center(e.Graphics, printingText, BOLD_FONT);
                size = GetStringSize(e.Graphics, printingText, BOLD_FONT);
                y += size.Height - 4;
                e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y);
            }

            //HALL Name
            //printingText = objInfo.Title.Trim();
            //x = X_Point_Center(e.Graphics, printingText, BOLD_FONT);
            //size = GetStringSize(e.Graphics, printingText, BOLD_FONT);
            //y += size.Height - 3;
            //e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y);

            //HALL ADDRESS
            printingText = PrintDetail.Address.Trim();
            x = X_Point_Center(e.Graphics, printingText, SEMIBOLD_FONT);
            y += size.Height - 3;
            e.Graphics.DrawString(printingText, SEMIBOLD_FONT, brush, x, y);

            //INVOICE TITLE

           // string taxInvoiceTitle = EnableTaxRegistration ? objInfo.TicketDetal.IsOriginalCopy ? "Abbreviated Invoice" : "Abbreviated Tax Invoice" : "Tax Invoice";
            string taxInvoiceTitle = "Tax Invoice";
            //if (!objInfo.IsTaxInvoice)
            //{
            //    if (objInfo.IsAbbreviated)
            //        taxInvoiceTitle = "Abbreviated " + taxInvoiceTitle;
            //}

            printingText = taxInvoiceTitle;
            x = X_Point_Center(e.Graphics, printingText, SEMIBOLD_FONT);
            size = GetStringSize(e.Graphics, printingText, SEMIBOLD_FONT);
            lineSpacing = size.Height;
            y += lineSpacing + 5;
            e.Graphics.DrawString(printingText, SEMIBOLD_FONT, brush, x, y);

            //S.NO Header
            printingText = "S.No. : ";
            x = LeftMargin;
            y += lineSpacing + 10;
            e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            //S.NO Value
            size = GetStringSize(e.Graphics, printingText + LeftMargin, LightFont);
            printingText = PrintDetail.SerialNumber.Trim();
            x = size.Width;
            e.Graphics.DrawString(printingText, SEMIBOLD_FONT, brush, x, y);

            //VAT NO Header. Value
            printingText = "Vat No. : " + PrintDetail.TheaterVatNo;
            x = X_Point_Right(e.Graphics, printingText, LightFont);
            e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            //if (objInfo.TicketDetal.IsBulkSale)
            //{
            //    //S.NO Header
            //    printingText = "Cus. Name: ";
            //    x = LeftMargin;
            //    y += lineSpacing;
            //    e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            //    //S.NO Value
            //    size = GetStringSize(e.Graphics, printingText + LeftMargin, LightFont);
            //    printingText = objInfo.TicketDetal.CustomerName.Trim();
            //    x = size.Width;
            //    e.Graphics.DrawString(printingText, SEMIBOLD_FONT, brush, x, y);


            //    //VAT NO Header. Value
            //    printingText = "Pan/VAT No. : " + objInfo.TicketDetal.PanNo;
            //    //x = X_Point_Right(e.Graphics, printingText, LightFont);
            //    x = leftMargin;
            //    y += lineSpacing;
            //    e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            //}

            printingText = "Movie : ";
            x = LeftMargin;
            y += lineSpacing;
            e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            size = GetStringSize(e.Graphics, printingText + LeftMargin, LightFont);
            printingText = printingText = ticket.Title.Trim() + (ticket.IsThreeD.Equals("1") ? " (3D)" : " (2D)");
            x = size.Width;
            e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y - 3);


            //Movie Name Header
            //printingText = "Movie : ";
            //x = LeftMargin;
            //y += lineSpacing;
            //e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            ////Movie Name Value
            //size = GetStringSize(e.Graphics, printingText + LeftMargin, LightFont);
            //string rollQuality = string.Empty;
            //printingText = ticket.Title.Trim() + (ticket.IsThreeD.Equals("1")?" 3D":"");
            ////x = size.Width;
            //e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y - 3);

            ////Screen Name Header
            //printingText = "Screen : ";
            //x = LeftMargin;
            //y += lineSpacing;
            //e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            ////Screen Name Value
            //size = GetStringSize(e.Graphics, printingText + LeftMargin, LightFont);
            //printingText = ticket.Screen.Trim();
            //x = size.Width;
            //e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y - 2);

            ////Type Value
            //printingText = PrintDetail.TicketType.Trim();
            //x = X_Point_Right(e.Graphics, printingText, SEMIBOLD_FONT);
            ////  size = GetStringSize(e.Graphics, printingText, SEMIBOLD_FONT);
            //e.Graphics.DrawString(printingText, SEMIBOLD_FONT, brush, x, y);
            ////Type Name
            //printingText = " Ticket Type : ";
            //size = GetStringSize(e.Graphics, printingText, LightFont);
            //e.Graphics.DrawString(printingText, SEMIBOLD_FONT, brush, x, y);



            printingText = "Screen. : ";
            x = LeftMargin;
            y += lineSpacing;
            e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            size = GetStringSize(e.Graphics, printingText + LeftMargin, LightFont);
            printingText = ticket.Screen.Trim();
            x = size.Width;
            e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y - 2);

            printingText = PrintDetail.TicketType.Trim();
            x = X_Point_Right(e.Graphics, printingText, SEMIBOLD_FONT);
            //  size = GetStringSize(e.Graphics, printingText, SEMIBOLD_FONT);
            e.Graphics.DrawString(printingText, SEMIBOLD_FONT, brush, x, y);
            //Type Name
            printingText = "Type : ";
            size = GetStringSize(e.Graphics, printingText, LightFont);
            e.Graphics.DrawString(printingText, LightFont, brush, x - size.Width, y - 1);

            //Date Header
            //printingText = "Date";
            //x = LeftMargin;
            //y += lineSpacing;
            //e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            printingText = "Date : ";
            x = LeftMargin;
            y += lineSpacing;
            e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            //Time Header
            printingText = "Time";
            x = X_Point_Center(e.Graphics, printingText, LightFont);
            e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            //Seat Number Header
            printingText = "Seat Number";
            x = X_Point_Right(e.Graphics, printingText, LightFont);
            e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            //Date
            //size = GetStringSize(e.Graphics, printingText + LeftMargin, LightFont);
            printingText = ticket.ShowDate.Trim();
            x = LeftMargin;
            y += lineSpacing;
            e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y);


            //Time Value
            printingText = ticket.Time;
            x = X_Point_Center(e.Graphics, printingText, BOLD_FONT);
            y += lineSpacing;
            e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y - 10);
            //printingText = "Time : ";
            //e.Graphics.DrawString(printingText, LightFont, brush, x, y);
            //x = X_Point_Right(e.Graphics, printingText, LightFont);
            //size = GetStringSize(e.Graphics, printingText + LeftMargin, LightFont);
            //printingText = ticket.Time.ToString("HH:mm tt");
            //x = size.Width;
            //e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y);
            //printingText = ticket.Time.ToString("HH:mm tt");
            //x = X_Point_Right(e.Graphics, printingText, SEMIBOLD_FONT);
            ////  size = GetStringSize(e.Graphics, printingText, SEMIBOLD_FONT);
            //e.Graphics.DrawString(printingText, SEMIBOLD_FONT, brush, x, y);
            //Type Name
            //printingText = " Ticket Type : ";
            //size = GetStringSize(e.Graphics, printingText, LightFont);
            //e.Graphics.DrawString(printingText, SEMIBOLD_FONT, brush, x, y);

            //Time Header
            //printingText = "Time";
            //x = X_Point_Center(e.Graphics, printingText, LightFont);
            //e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            //Seat Number Header
            //printingText = "Seat Number";
            //x = X_Point_Right(e.Graphics, printingText, LightFont);
            //e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            //Date Value
            //printingText = ticket.ShowDate.ToString("MMM-dd-yyyy").Trim();
            //x = LeftMargin;
            //y += lineSpacing;
            //e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y);

            //Time Value
            //printingText = ticket.Time.ToString("HH:mm tt");
            //x = X_Point_Center(e.Graphics, printingText, BOLD_FONT);
            //e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y);

            //Seat Number Value
            //printingText = PrintDetail.SeatNo.Trim();
            //x = X_Point_Right(e.Graphics, printingText, LARGE_BOLD_FONT) - 10;
            //e.Graphics.DrawString(printingText, LARGE_BOLD_FONT, brush, x, y);

            if (StaticData.ThreeDCharge > 0) y += 2.25F;
            else y += 3.5F;


            //if (printType == "qrcode")
            //{
            //    img = Utilities.ResizeImage(img, new Size(200, 80), true);
            //    x = X_Point_Left(e.Graphics, img) + 10;
            //    y += size.Height + lineSpacing - 5;
            //    e.Graphics.DrawImage(img, x, y);
            //}
            //else

            //{
            //    y += 20;
            //}
            if(StaticData.PrintType == "qrcode")
            {
                img = Utilities.ResizeImage(img, new Size(200, 80), true);
                x = X_Point_Left(e.Graphics, img) + 10;
                y += size.Height + lineSpacing - 5;
                e.Graphics.DrawImage(img, x, y);
            }
            else

            {
                y += 25;
            }
            if (!StaticData.EnableTaxRegistration)
            {
                printingText = "Entrance Fee : Rs.";
            }
            else
            {
                printingText = "Entrance Fee (Incl. Vat) : Rs.";
            }

            if (StaticData.EnableTaxRegistration)
                printingText = printingText + (EntranceFee + VAT).ToString("0.00");
            else
                printingText = printingText + EntranceFee.ToString("0.00");

            //Entrance Fee Name.Value



            x = X_Point_Right(e.Graphics, printingText, LightFont);
            y += lineSpacing - 20;
            e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            if (ThreeDCharge != 0)
            {
                printingText = "3D.Charge: Rs." + ThreeDCharge.ToString("0.00");
                y += lineSpacing;
                x = X_Point_Right(e.Graphics, printingText, LightFont);
                e.Graphics.DrawString(printingText, LightFont, brush, x, y);


            }
            if (FDF != 0)
            {
                printingText = "FDF(" + (StaticData.BoxOfficeTax * 100).ToString("#") + "%): Rs." + FDF.ToString("0.00");
                y += lineSpacing;
                x = X_Point_Right(e.Graphics, printingText, LightFont);
                e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            }

            if (LocalTax != 0)
            {
                printingText = "Local Tax(" + (StaticData.LocalTax * 100).ToString("#") + "%): Rs." + LocalTax.ToString("0.00");
                y += lineSpacing;
                x = X_Point_Right(e.Graphics, printingText, LightFont);
                e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            }
            //if (objInfo.TicketDetal.ConvinienceCharge != 0)
            //{
            //    printingText = "C.Charge: Rs." + objInfo.TicketDetal.ConvinienceCharge.ToString("0.00");
            //    y += lineSpacing;
            //    x = X_Point_Right(e.Graphics, printingText, LightFont);
            //    e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            //}
            //if (discount > 0)
            //{
            //    printingText = "Discount : " + discount.ToString("0.00");
            //    y += lineSpacing;
            //    x = X_Point_Right(e.Graphics, printingText, LightFont);
            //    e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            //}
            if (VAT != 0 && !EnableTaxRegistration)
            {
                printingText = "VAT(" + (StaticData.EntertainmentTax * 100).ToString("#") + "%): Rs." + VAT.ToString("0.00");
                y += lineSpacing;
                x = X_Point_Right(e.Graphics, printingText, LightFont);
                e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            }
            printingText = "Total Cost : " + TotalCost.ToString("0.00");

            y += lineSpacing;
            x = X_Point_Right(e.Graphics, printingText, LightFont);
            e.Graphics.DrawString(printingText, LightFont, brush, x, y);

            if (TotalCost == 0)
            {
                printingText = "COMPLIMENTARY";
                if (StaticData.PrintType == "barcode")
                {
                    y += lineSpacing + 00F;
                    x = X_Point_Center(e.Graphics, printingText, BOLD_FONT);
                }
                else
                {
                    y += lineSpacing + 20F;
                    x = X_Point_Center(e.Graphics, printingText, BOLD_FONT) + 30;
                }

                e.Graphics.DrawString(printingText, LARGE_BOLD_FONT, brush, x, y);

                if (StaticData.PrintType == "barcode")
                    y += lineSpacing - 5.5F;
                else
                    y += lineSpacing - 33.5F;
            }

            //Bar Code
           // logo = objInfo.BarcodeImage;
            if (printType == "barcode")
            {
                BarcodeHelper objHelper = new BarcodeHelper();
                Image barCodeImage = objHelper.GenerateBarCode(barcode);
                x = X_Point_Center(e.Graphics, barCodeImage);
                y += lineSpacing + 10;
                e.Graphics.DrawImage(barCodeImage, x, y);

                y += barCodeImage.Height - 30;
            }
            else
            {
                y += 10;
            }
            bool reprint = true;
            //if (printType != "barcode")
            //{
            //    x = X_Point_Center(e.Graphics, printingText, BOLD_FONT) + 10;
            //    e.Graphics.DrawString(printingText, BOLD_FONT, brush, x, y);
            //}

            x = LeftMargin;
            if (printType == "barcode" && TotalCost == 0 && ticket.IsThreeD == "3D")
                y += 2.5F;
            else
                y += 10.5F;
            if (ThreeDCharge == 0) y += 12.5F;
            if (FDF == 0) y += 12.5F;
            if (StaticData.TermsAndConditions != string.Empty)
            {

                y += lineSpacing;

                //y += lineSpacing;
                int len = StaticData.TermsAndConditions.Length;
                //Terms And Conditions
                printingText = StaticData.TermsAndConditions;
                int a = printingText.Count(c => c == '\n');
                int height = (a < 3) ? 13 * (a + 1) : 13 * a;
                RectangleF rectangle = new RectangleF((int)x, (int)y, (int)PageWidth - RightMargin, height);
                e.Graphics.DrawString(printingText, semiLightBoldFont, brush, rectangle);
                //e.Graphics.DrawRectangle(Pens.Black, Rectangle.Round(rectangle));

                y += height;
            }

            if (printType == "barcode" && TotalCost == 0 && ticket.IsThreeD != "3D")
                y += 15.5F;


            if (ticket.IsThreeD == "3D")
            {
                y += 0;
                printingText = DataHolder.ThreeDMessage;
                x = X_Point_Center(e.Graphics, printingText, semiLightBoldFont5F);
                e.Graphics.DrawString(printingText, semiLightBoldFont5F, brush, x, y);

                y += 23;
            }

            if (ThreeDCharge == 0 && TotalCost > 0)
                //y += logoHeight / 3 - 5.25F;
                y += -3.5F;
            else if (ThreeDCharge == 0 && TotalCost == 0 && printType == "barcode")
                y += -3.0F;
            else if (ThreeDCharge == 0 && TotalCost == 0 && ticket.IsThreeD != "3D")
                y += 21F;
            else if (ThreeDCharge == 0 && TotalCost == 0 && ticket.IsThreeD == "3D")
                y += -1.5F;
            else
                y += -3.5F;

            y += lineSpacing + 5;
            //printingText = "Printed By: " + DataHolder.UserName;
            //x = X_Point_Left(e.Graphics, printingText, semiLightBoldFont);

            //e.Graphics.DrawString(printingText, semiLightBoldFont, brush, x, y);
            //y += lineSpacing - 5;

            ////Todays Date
            //size = GetStringSize(e.Graphics, printingText, semiLightFont);
            //printingText = DateTime.Now.ToString("yyyy/MM/dd hh:mm tt");
            //x = X_Point_Left(e.Graphics, printingText, semiLightBoldFont);

            //e.Graphics.DrawString(printingText, semiLightBoldFont, brush, x, y);

            //printingText = "Enjoy your movie at " + DataHolder.HallName;
            //x = X_Point_Right(e.Graphics, printingText, semiLightBoldFont);
            //e.Graphics.DrawString(printingText, semiLightBoldFont, brush, x, y);
            //LOGO

        }

        /// <summary>
        /// Get the starting X point of the text 'Center Page'
        /// </summary>
        private float X_Point_Center(Graphics g, string text, Font font)
        {
            SizeF stringSize = GetStringSize(g, text, font);
            float x = (PageWidth - stringSize.Width) / 2;
            return x;
        }

        /// <summary>
        /// Get the starting X point of the Image 'Center Page'
        /// </summary>
        private float X_Point_Center(Graphics g, Image image)
        {
            float x = (PageWidth - image.Width) / 2;
            return x;
        }

        private float X_Point_Left(Graphics g, Image image)
        {
            float x = 0;
            return x;
        }

        /// <summary>
        /// Get the starting X point of the text ''Right Side'
        /// </summary>
        private float X_Point_Right(Graphics g, string text, Font font)
        {
            SizeF size = GetStringSize(g, text, font);
            float x = (PageWidth - (size.Width + RightMargin));
            return x;
        }

        private float X_Point_Left(Graphics g, string text, Font font)
        {
            SizeF size = GetStringSize(g, text, font);
            float x = 0;
            return x;
        }
        
        #endregion
    }
}
