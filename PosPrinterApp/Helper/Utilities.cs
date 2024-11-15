using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace PosPrinterApp.Helper
{
    public class Utilities
    {
        /// <summary>
        /// Old School
        /// </summary>
        /// <param name="catchedControl"></param>
        /// <returns></returns>
        

        public static string GenerateAlphaNumericText(int length)
        {
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(DataHolder.AlphaNumericTextGenerator, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

        public static string GenerateRandomCode(int length)
        {
            Random random = new Random();
            string s = "";
            string[] CapchaValue = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            for (int i = 0; i < length; i++)
                s = String.Concat(s, CapchaValue[random.Next(10)]);
            return s;
        }

        public static Image GetAppImage()
        {
            Image img = Image.FromFile(StaticData.LogoPath);

            if (DataHolder.AppLogo != null)
                img = DataHolder.AppLogo;
            return img;
            //return ResizeImage(img, new Size(139, 67), true);
        }

        public static string GetFormat2Price(decimal unit)
        {
            return string.Format(US.RsFormat2, unit.ToString(US.SF_OpointOO));
        }

        public static Image GetLogo()
        {
            string imageString = DataHolder.ImageString;
            Image logo = Utilities.GetAppImage();
            //  Image logo = Utilities.StringToImage(imageString);

            logo = Utilities.ResizeImage(logo, new Size(170, 100), true);
            return logo;
        }
        public static Image GetPrintLogo()
        {
            string imageString = DataHolder.ImageString;
            Image img = Image.FromFile(StaticData.LogoPath);
            if (DataHolder.PrintLogo != null)
                img = DataHolder.PrintLogo;
            img = Utilities.ResizeImage(img, new Size(220, 80), true);
            return img;
        }
        public static string FormatedDate(DateTime date)
        {
            string dateFormated = string.Empty;
            dateFormated = date.ToString("MMM") + " " + date.Day.ToString() + ", " + date.Year.ToString();
            return dateFormated;
        }
        public static Image GenerateBarCode(string code, int width, int height)
        {
            //Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            //Image imgBarCode = barCode.Draw(code, maxHeight);

            BarcodeLib.Barcode bCode = new BarcodeLib.Barcode();
            bCode.IncludeLabel = true;
            //bCode.BackColor = Color.FromArgb(230, 230, 230);
            Image imgBarCode = bCode.Encode(BarcodeLib.TYPE.CODE128C, code, width, height);
            return imgBarCode;
        }
        //public static List<PaymentTypeInfo> GetPaymentType(bool IsTicket = true)
        //{
        //    CineRestroStaffClient client = new CineRestroStaffClient();
        //    var serviceURi = new Uri(DataHolder.APIURL);
        //    client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceURi, client.Endpoint.Address.Identity, client.Endpoint.Address.Headers);
        //    List<SageFlick.CineRestroService.PaymentTypeInfo> paymentTypeInfo = client.GetPaymentTypeList(DataHolder.UserName).ToList();
        //    List<PaymentTypeInfo> lstPaymentTypeInfo = new List<PaymentTypeInfo>();
        //    foreach (var item in paymentTypeInfo)
        //    {
        //        if (IsTicket)
        //            lstPaymentTypeInfo.Add(new PaymentTypeInfo() { PaymentTypeID = item.PaymentTypeID, PaymentTypeName = item.Title, PaymentTypeTitle = item.Alias });
        //        else if (item.Title.ToLower() != "online transfer")
        //            lstPaymentTypeInfo.Add(new PaymentTypeInfo() { PaymentTypeID = item.PaymentTypeID, PaymentTypeName = item.Title, PaymentTypeTitle = item.Alias });
        //    }
        //    //lstPaymentTypeInfo.Add(new PaymentTypeInfo() { PaymentTypeID = 6, PaymentTypeName = "Cash", PaymentTypeTitle = "Cash" });
        //    //lstPaymentTypeInfo.Add(new PaymentTypeInfo() { PaymentTypeID = 7, PaymentTypeName = "Electronic Card", PaymentTypeTitle = "Electronic Card" });
        //    //// lstPaymentTypeInfo.Add(new PaymentTypeInfo() { PaymentTypeID = 2, PaymentTypeName = "Membership Card", PaymentTypeTitle = "Card" });

        //    //if (DataHolder.UserType == 2 || DataHolder.UserType == 4 || DataHolder.UserType == 7 || DataHolder.UserType == 8 || DataHolder.UserType == 9)
        //    //{
        //    //    lstPaymentTypeInfo.Add(new PaymentTypeInfo() { PaymentTypeID = 8, PaymentTypeName = "Complimentary", PaymentTypeTitle = "Complimentary" });
        //    //}

        //    return lstPaymentTypeInfo;
        //}

        public static string GetServiceTaxRatePreHeder()
        {
            return "Service Tax (" + (DataHolder.ServiceTaxRate * 100).ToString("F0") + "% ) : ";
        }

       

        public static string GetVatRatePreHeader()
        {
            return "VAT(" + (DataHolder.VatRate * 100).ToString("F0") + "% ) : ";
        }

        public static bool IsConnectedToInternet()
        {
            //int desc;
            //return InternetGetConnectedState(out desc, 0);
            return true;
        }

        public static bool NumericValidation(string number)
        {
            bool validation = Regex.IsMatch(number, Validate.NumberConstraints);
            return validation;
        }

        public static bool PriceValidation(string number)
        {
            bool validation = false;
            if (number.Contains("."))
                validation = Regex.IsMatch(number, Validate.DecimalConstraints);
            else
                validation = Regex.IsMatch(number, Validate.NumberConstraints);

            return validation;
        }

       

        public Image CreateTicketImage(string ticketString)
        {
            byte[] bArray1 = Convert.FromBase64String(ticketString);
            MemoryStream ms = new MemoryStream(bArray1);
            Image ticket = Image.FromStream(ms, true, true);
            ticket = ResizeImage(ticket, new Size(350, 400));
            return ticket;
        }

        public static Image ResizeImage(Image image, Size size, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)size.Width / (float)originalWidth;
                float percentHeight = (float)size.Height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public static Bitmap ResizeImage(Bitmap image, Size size, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)size.Width / (float)originalWidth;
                float percentHeight = (float)size.Height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        //public static void ShowNetworkError()
        //{
        //    MessageBox.Show(MessageManager.MSG_CONNECTION_STATUS, MessageManager.MSG_CONNECTION_ERROR, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //}

        public static Image StringToImage(string imageString)
        {
            if (imageString.Contains(","))
            {
                imageString = imageString.Split(',')[1];
            }
            byte[] bArray1 = Convert.FromBase64String(imageString);
            MemoryStream ms = new MemoryStream(bArray1);
            Image logo = Image.FromStream(ms, true, true);
            return logo;
        }

        public string GenerateRandomText(int length)
        {
            var random = new Random(length);
            var result = new string(
                Enumerable.Repeat(DataHolder.RandomTextGenerator, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        //public static List<PaymentTypeInfo> GetOnlinePaymentType()
        //{
        //    CineRestroStaffClient client = new CineRestroStaffClient();
        //    var serviceURi = new Uri(DataHolder.APIURL);
        //    client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceURi, client.Endpoint.Address.Identity, client.Endpoint.Address.Headers);



        //    List<SageFlick.CineRestroService.PaymentTypeInfo> paymentTypeInfo = client.GetOnlinePaymentTypeList(DataHolder.UserName).ToList();



        //    List<PaymentTypeInfo> lstPaymentTypeInfo = new List<PaymentTypeInfo>();
        //    foreach (var item in paymentTypeInfo)
        //    {
        //        lstPaymentTypeInfo.Add(new PaymentTypeInfo() { PaymentTypeID = item.PaymentTypeID, PaymentTypeName = item.Title, PaymentTypeTitle = item.Alias });
        //    }

        //    return lstPaymentTypeInfo;
        //}

        //public static List<SeatsByTicketType> GetSeatsByTicketType(int showID)
        //{
        //    CineRestroStaffClient client = new CineRestroStaffClient();
        //    var serviceURi = new Uri(DataHolder.APIURL);
        //    client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceURi, client.Endpoint.Address.Identity, client.Endpoint.Address.Headers);
        //    List<SeatsByTicketType> paymentTypeInfo = client.GetSeatsInfoByTicketType(DataHolder.GetMacAddress(), showID).ToList();


        //    return paymentTypeInfo;
        //}

        //public static List<SeatViewMode> GetSeatViewModes()
        //{

        //    SeatViewMode defaultSeatViewMode = new SeatViewMode { SeatViewModeID = 0, ViewMode = "Default" };
        //    SeatViewMode bulkSeatViewMode = new SeatViewMode { SeatViewModeID = 1, ViewMode = "Bulk" };
        //    List<SeatViewMode> seatViewModes = new List<SeatViewMode>();
        //    seatViewModes.Add(defaultSeatViewMode);
        //    seatViewModes.Add(bulkSeatViewMode);

        //    return seatViewModes;
        //}
    }
}