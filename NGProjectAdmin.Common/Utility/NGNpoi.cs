using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// NPOI工具类
    /// </summary>
    public static class NGNpoi
    {
        #region 单元格添加批注

        /// <summary>
        /// 单元格添加批注
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="commentStr">批注</param>
        public static void SetCellComment(this ISheet worksheet, int row, int column, String commentStr)
        {
            if (worksheet.GetRow(row).GetCell(column).CellComment == null)
            {
                HSSFPatriarch patr = (HSSFPatriarch)worksheet.CreateDrawingPatriarch();
                HSSFComment comment = (HSSFComment)patr.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, column, row - 1, column + 3, row + 4));
                comment.String = new HSSFRichTextString(commentStr);
                comment.Author = @"RuYiAdmin";
                worksheet.GetRow(row).GetCell(column).CellComment = comment;
            }
            else
            {
                var comment = worksheet.GetRow(row).GetCell(column).CellComment;
                var coStr = comment.String;
                comment.String = new HSSFRichTextString(commentStr + System.Environment.NewLine + coStr);
                comment.Author = @"RuYiAdmin";
                worksheet.GetRow(row).GetCell(column).CellComment = comment;
            }
        }

        #endregion

        #region 获取单元格的值

        /// <summary>
        /// 获取单元格的值
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>字符串</returns>
        public static String GetCellValue(this ICell cell)
        {
            String result = String.Empty;

            if (cell == null)
            {
                return result;
            }

            switch (cell.CellType)
            {
                case CellType.Unknown:
                    break;
                case CellType.Numeric:
                    result = cell.NumericCellValue.ToString();
                    break;
                case CellType.String:
                    result = cell.StringCellValue;
                    break;
                case CellType.Formula:
                    result = cell.CellFormula;
                    break;
                case CellType.Blank:
                    break;
                case CellType.Boolean:
                    result = cell.BooleanCellValue.ToString();
                    break;
                case CellType.Error:
                    result = cell.ErrorCellValue.ToString();
                    break;
                default: break;
            }

            return result;
        }

        #endregion

        #region 保存Excel

        /// <summary>
        /// 保存Excel
        /// </summary>
        /// <param name="workbook">工作簿</param>
        /// <param name="xlxPath">excel路径</param>
        /// <returns>新路径</returns>
        public static String SaveAsXlx(this IWorkbook workbook, String xlxPath)
        {
            var xlxFile = new FileInfo(xlxPath);

            var newXlx = Guid.NewGuid() + Path.GetExtension(xlxFile.FullName);
            var filePath = Path.Join(xlxFile.Directory + "/", newXlx);

            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);

            workbook.Write(fileStream);
            workbook.Close();

            fileStream.Close();

            return filePath;
        }

        #endregion
    }
}
