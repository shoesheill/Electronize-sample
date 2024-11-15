using MessagingToolkit.QRCode.Codec;
using System.Drawing;
using System.Drawing.Imaging;

namespace PosPrinterApp.Helper
{

    public class ImageHelper
    {
        string CodeType = "barcode";

        public ImageHelper()
        {
            CodeType = "qrcode";
        }

        public string GenerateImage(string Code, string FilePath)
        {
            if (CodeType == "qrcode")
            {
                return GenerateQRImage(Code, FilePath);
            }
            else
            {
                return GenerateBarCodeImg(Code, FilePath);
            }

        }

        public string GenerateQRImage(string code, string filePath)
        {
            string QRfile = string.Empty;
            QRCodeEncoder encoder = new QRCodeEncoder();

            Bitmap img = encoder.Encode(code);
            QRfile = code + ".jpg";
            string DestinationFolder = filePath;
            if (!Directory.Exists(DestinationFolder))
            {
                Directory.CreateDirectory(DestinationFolder);
            }
            if (code != "")
            {
                img.Save(DestinationFolder + code + ".jpg", ImageFormat.Jpeg);
            }
            return filePath + QRfile;
        }

        public string GenerateBarCodeImg(string code, string filePath)
        {
            string barcodeFileLocation = filePath;
            string barcodeFilePath = barcodeFileLocation + code + ".png";
            string barFiles = string.Empty;
            barFiles = code + ".png";
            if (!Directory.Exists(barcodeFileLocation))
            {
                Directory.CreateDirectory(barcodeFileLocation);
            }

            if (!File.Exists(barcodeFilePath))
            {
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                b.Width = 300;
                b.Height = 80;
                b.IncludeLabel = true;
                b.LabelPosition = BarcodeLib.LabelPositions.TOPCENTER;
                BarcodeLib.TYPE bType = BarcodeLib.TYPE.CODE128;

                System.Drawing.Image img = b.Encode(bType, code);

                img.Save(barcodeFilePath, ImageFormat.Png);
            }
            return filePath + barFiles;
        }

        public Image GenerateImageBitMap(string code, string filePath)
        {
            if (CodeType == "qrcode")
            {
                return GenerateQRImageBitMap(code, filePath);
            }
            else
            {
                return GenerateBarCodeImgBitMap(code, filePath);
            }
        }

        private Image GenerateBarCodeImgBitMap(string code, string filePath)
        {
            string barcodeFileLocation = filePath;
            string barcodeFilePath = barcodeFileLocation + code + ".png";
            string barFiles = string.Empty;
            barFiles = code + ".png";
            if (!Directory.Exists(barcodeFileLocation))
            {
                Directory.CreateDirectory(barcodeFileLocation);
            }

            System.Drawing.Image img1 = null;

            if (!File.Exists(barcodeFilePath))
            {
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                b.Width = 300;
                b.Height = 80;
                b.IncludeLabel = true;
                b.LabelPosition = BarcodeLib.LabelPositions.TOPCENTER;
                BarcodeLib.TYPE bType = BarcodeLib.TYPE.CODE128;

                img1 = b.Encode(bType, code);

                img1.Save(barcodeFilePath, ImageFormat.Png);

                //img1 = b.Generate_Image();
            }
            return img1;
        }

        private Bitmap GenerateQRImageBitMap(string code, string filePath)
        {
            string QRfile = string.Empty;
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap img = encoder.Encode(code);
            QRfile = code + ".jpg";
            string DestinationFolder = filePath;// HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(DestinationFolder))
            {
                Directory.CreateDirectory(DestinationFolder);
            }
            if (code != "")
            {
                img.Save(DestinationFolder + code + ".jpg", ImageFormat.Jpeg);
            }
            return img;
        }


        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// </summary>
        /// <param name=""image"">The <see cref=""System.Drawing.Image"/"> to rotate
        /// <param name=""offset"">The position to rotate from.
        /// <param name=""angle"">The amount to rotate the image, clockwise, in degrees
        /// <returns>A new <see cref=""System.Drawing.Bitmap"/"> of the same size rotated.</see>
        /// <exception cref=""System.ArgumentNullException"">Thrown if <see cref=""image"/"> 
        /// is null.</see>
        public Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        public Bitmap ScaleImage(Image bmp, float percentage)
        {

            var ratioX = (double)percentage / 100.0;
            var ratioY = (double)percentage / 100.0;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(bmp.Width * ratio);
            var newHeight = (int)(bmp.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(bmp, 0, 0, newWidth, newHeight);

            return newImage;
        }

    }

}
