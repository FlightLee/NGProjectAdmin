using System;
using System.IO;
using Tesseract;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// OCR工具类
    /// </summary>
    public static class NGOCR
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static String Recognize(String imagePath)
        {
            String result = String.Empty;

            using (var ocr = new TesseractEngine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Library/Tesseract"), "chi_sim", EngineMode.Default))
            {
                var pix = Pix.LoadFromFile(imagePath);
                using (var page = ocr.Process(pix))
                {
                    String text = page.GetText();
                    if (!String.IsNullOrEmpty(text))
                    {
                        result = text;
                    }
                }
            }

            return result;
        }
    }
}
