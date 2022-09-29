using NGProjectAdmin.Common.Global;
using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// ZXing工具类
    /// </summary>
    public class RuYiZXingCodeUtil
    {

        /// <summary>
        /// /生成二维码
        /// </summary>
        /// <param name="message">二维码内容</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="width">二维码宽度</param>
        /// <param name="height">二维码高度</param>
        /// <returns>文件路径</returns>
        public static String CreateQrCode(String message, String fileName, int width = 300, int height = 300)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return String.Empty;
            }

            int hei = width;
            if (width > height)
            {
                hei = height;
                width = height;
            }

            String filePath = String.Join('/', NGAdminGlobalContext.DirectoryConfig.GetTempPath(), fileName + ".gif");

            var qRCodeWriter = new ZXing.QrCode.QRCodeWriter();
            BitMatrix bitMatrix = qRCodeWriter.encode(message, BarcodeFormat.QR_CODE, width, hei);

            var webBarcodeWriter = new ZXing.ZKWeb.BarcodeWriter();
            webBarcodeWriter.Options = new EncodingOptions() { Margin = 0 };

            Bitmap bitmap = webBarcodeWriter.Write(bitMatrix);
            bitmap.Save(filePath, ImageFormat.Gif);
            bitmap.Dispose();

            return filePath;
        }

        /// <summary>
        /// 读取二维码或者条形码图片内容
        /// </summary>
        /// <param name="imgFile">二维码或者条形码路径</param>
        /// <returns>文本</returns>
        public static String ReadFromImage(String imgFile)
        {
            if (String.IsNullOrWhiteSpace(imgFile))
            {
                return String.Empty;
            }

            Image img = Image.FromFile(imgFile);
            Bitmap bitmap = new Bitmap(img);

            var barcodeReader = new ZXing.ZKWeb.BarcodeReader();
            barcodeReader.Options = new DecodingOptions { CharacterSet = "UTF-8" };

            Result r = barcodeReader.Decode(bitmap);
            String resultText = r.Text;

            bitmap.Dispose();
            img.Dispose();

            return resultText;

        }

        /// <summary>
        /// BitmapToArray
        /// </summary>
        /// <param name="bmp">Bitmap</param>
        /// <returns>byte[]</returns>
        public static byte[] BitmapToArray(Bitmap bmp)
        {
            byte[] byteArray = null;
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                byteArray = stream.GetBuffer();
            }
            return byteArray;
        }

        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="message">条形码内容</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="width">条形码宽度</param>
        /// <param name="height">条形码高度</param>
        /// <returns>文件路径</returns>
        public static String CreateBarcode(String message, String fileName, int width = 400, int height = 100)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return String.Empty;
            }

            String filePath = String.Join('/', NGAdminGlobalContext.DirectoryConfig.GetTempPath(), fileName + ".gif");

            var codaBarWriter = new ZXing.OneD.CodaBarWriter();
            BitMatrix bitMatrix = codaBarWriter.encode(message, BarcodeFormat.CODABAR, width, height);
            var webBarcodeWriter = new ZXing.ZKWeb.BarcodeWriter();
            webBarcodeWriter.Options = new EncodingOptions() { Margin = 3, PureBarcode = false };

            Bitmap bitmap = webBarcodeWriter.Write(bitMatrix);
            bitmap.Save(filePath, ImageFormat.Gif);
            bitmap.Dispose();

            return filePath;
        }
    }
}
