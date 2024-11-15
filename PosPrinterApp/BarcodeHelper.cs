using BarcodeLib;
using System.Drawing;

namespace TicketApp
{
    public class BarcodeHelper
    {
        public Image GenerateBarCode(string ticketCode)
        {

            Barcode b = new Barcode();
            b.Width = 200;
            b.Height = 80;
            b.IncludeLabel = true;
            b.LabelPosition = LabelPositions.BOTTOMCENTER;
            TYPE bType = BarcodeLib.TYPE.UPCA;
            Image img = b.Encode(bType, ticketCode);
            // b.SaveImage("D:\\Cine.SageFrame.Retro\\grush\\Cine.SageFrame\\Entity\\test" + randomText + ".png", SaveTypes.PNG);

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] bt = ms.ToArray();

            return img;
        }
    }
}
