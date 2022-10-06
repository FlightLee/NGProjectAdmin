
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 导入配置DTO
    /// </summary>
    public class ImportConfigDTO : SysImportConfig
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public String ExcelPath { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<ImportConfigDetailDTO> Children { get; set; }

        #region 公有方法

        /// <summary>
        /// 有效性校验
        /// </summary>
        /// <returns>错误数量</returns>
        public int ValidationDetecting()
        {
            var errorCount = 0;

            if (String.IsNullOrEmpty(this.ExcelPath))
            {
                throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("file can not be empty");
            }

            var oldStream = new FileStream(this.ExcelPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            var workbook = new HSSFWorkbook(oldStream);

            if (!String.IsNullOrEmpty(this.WorkSheetIndexes))
            {
                var array = this.WorkSheetIndexes.Split(',');
                foreach (var worksheet in array)
                {
                    errorCount += this.DetectingWorksheet(workbook.GetSheetAt(int.Parse(worksheet)));
                }
            }
            else
            {
                errorCount += this.DetectingWorksheet(workbook.GetSheetAt(0));
            }

            var file = new FileInfo(this.ExcelPath);

            var newFile = Guid.NewGuid() + Path.GetExtension(file.FullName);

            var newPath = Path.Join(file.Directory + "/", newFile);

            var newStream = new FileStream(newPath, FileMode.Create, FileAccess.ReadWrite);

            workbook.Write(newStream);

            oldStream.Close();
            workbook.Close();
            file.Delete();

            newStream.Close();

            this.ExcelPath = newPath;

            return errorCount;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 校验工作簿
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        /// <returns>错误数量</returns>
        private int DetectingWorksheet(ISheet worksheet)
        {
            var errorCount = 0;

            foreach (var item in this.Children)
            {
                #region 校验必填项

                if (item.Rquired.Equals(1))
                {
                    var cols = item.Cells.Split(',');
                    for (var i = this.StartRow; i <= worksheet.LastRowNum; i++)
                    {
                        for (var j = this.StartColumn; j < worksheet.GetRow(this.StartRow).LastCellNum; j++)
                        {
                            if (cols.Contains(j.ToString()))
                            {
                                var value = worksheet.GetRow(i).GetCell(j).GetCellValue();
                                if (String.IsNullOrEmpty(value))
                                {
                                    errorCount++;
                                    worksheet.SetCellComment(i, j, "单元格的值不能为空！");
                                }
                            }
                        }
                    }
                }

                #endregion

                #region 按数据类型校验

                switch (item.DataType)
                {
                    case CellDataType.Decimal:
                        errorCount += this.DecimalValidation(worksheet, item);
                        break;

                    case CellDataType.Integer:
                        errorCount += this.IntegerValidation(worksheet, item);
                        break;

                    case CellDataType.Text:
                        errorCount += this.TextValidation(worksheet, item);
                        break;

                    case CellDataType.Date:
                        errorCount += this.DateValidation(worksheet, item);
                        break;

                    case CellDataType.DateTime:
                        errorCount += this.DateTimeValidation(worksheet, item);
                        break;

                    default: break;
                }

                #endregion
            }

            return errorCount;
        }

        /// <summary>
        /// 校验小数
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        /// <param name="detailConfig">配置明细</param>
        /// <returns>错误数量</returns>
        private int DecimalValidation(ISheet worksheet, ImportConfigDetailDTO detailConfig)
        {
            var errorCount = 0;

            #region 校验小数、最大值、最小值、小数位数

            var cols = detailConfig.Cells.Split(',');

            for (var i = this.StartRow; i <= worksheet.LastRowNum; i++)
            {
                for (var j = this.StartColumn; j < worksheet.GetRow(this.StartRow).LastCellNum; j++)
                {
                    var value = worksheet.GetRow(i).GetCell(j).GetCellValue();

                    //校验小数、最大值、最小值
                    if (cols.Contains(j.ToString()) && !String.IsNullOrEmpty(value))
                    {
                        #region 校验小数

                        if (!NGDigitUtil.IsNumber(value))
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, j, "单元格的值不是数字！");
                        }

                        #endregion

                        #region 校验最大值

                        if (NGDigitUtil.IsNumber(value) && detailConfig.MaxValue != null && Double.Parse(value.ToString()) > detailConfig.MaxValue)
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, j, "单元格的值不能大于最大值！");
                        }

                        #endregion

                        #region 校验最小值

                        if (NGDigitUtil.IsNumber(value) && detailConfig.MinValue != null && Double.Parse(value.ToString()) < detailConfig.MinValue)
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, j, "单元格的值不能小于最小值！");
                        }

                        #endregion
                    }

                    //校验小数位数
                    if (cols.Contains(j.ToString()) && NGDigitUtil.IsNumber(value) && detailConfig.DecimalLimit != null &&
                        !String.IsNullOrEmpty(value) && value.Contains(".") && value.ToString().Split('.')[1].Length > detailConfig.DecimalLimit)
                    {
                        errorCount++;
                        worksheet.SetCellComment(i, j, "小数位数超限！");
                    }
                }
            }

            #endregion

            return errorCount;
        }

        /// <summary>
        /// 校验整数
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        /// <param name="detailConfig">配置明细</param>
        /// <returns>错误数量</returns>
        private int IntegerValidation(ISheet worksheet, ImportConfigDetailDTO detailConfig)
        {
            var errorCount = 0;

            #region 校验整数、最大值、最小值、枚举

            var cols = detailConfig.Cells.Split(',');
            var enumCols = detailConfig.TextEnum != null && detailConfig.TextEnum.Length > 0 ? detailConfig.TextEnum.Split(',') : new string[0];

            for (var i = this.StartRow; i <= worksheet.LastRowNum; i++)
            {
                for (var j = this.StartColumn; j < worksheet.GetRow(this.StartRow).LastCellNum; j++)
                {
                    var value = worksheet.GetRow(i).GetCell(j).GetCellValue();

                    //校验整数、最大值、最小值
                    if (cols.Contains(j.ToString()) && !String.IsNullOrEmpty(value))
                    {
                        #region 校验整数

                        if (!NGDigitUtil.IsNumber(value))
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, j, "单元格的值不是数字！");
                        }

                        if (!NGDigitUtil.IsInt(value))
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, j, "单元格的值不是整数！");
                        }

                        #endregion

                        #region 校验最大值

                        if (NGDigitUtil.IsNumber(value) && detailConfig.MaxValue != null && Double.Parse(value.ToString()) > detailConfig.MaxValue)
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, j, "单元格的值不能大于最大值！");
                        }

                        #endregion

                        #region 校验最小值

                        if (NGDigitUtil.IsNumber(value) && detailConfig.MinValue != null && Double.Parse(value.ToString()) < detailConfig.MinValue)
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, j, "单元格的值不能小于最小值！");
                        }

                        #endregion
                    }

                    //校验枚举
                    if (cols.Contains(j.ToString()) && !String.IsNullOrEmpty(value) && enumCols.Length > 0 && !enumCols.Contains(value.ToString()))
                    {
                        errorCount++;
                        worksheet.SetCellComment(i, j, "单元格的值不在枚举列表中！");
                    }
                }
            }

            #endregion

            return errorCount;
        }

        /// <summary>
        /// 校验文本
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        /// <param name="detailConfig">配置明细</param>
        /// <returns>错误数量</returns>
        private int TextValidation(ISheet worksheet, ImportConfigDetailDTO detailConfig)
        {
            var errorCount = 0;

            #region 校验枚举

            if (detailConfig.TextEnum != null && detailConfig.TextEnum.Length > 0)
            {
                var cols = detailConfig.Cells.Split(',');
                var enumCols = detailConfig.TextEnum.Split(',');

                for (var i = this.StartRow; i <= worksheet.LastRowNum; i++)
                {
                    for (var j = this.StartColumn; j < worksheet.GetRow(this.StartRow).LastCellNum; j++)
                    {
                        var value = worksheet.GetRow(i).GetCell(j).GetCellValue();

                        //校验枚举
                        if (cols.Contains(j.ToString()) && enumCols.Length > 0 && !String.IsNullOrEmpty(value) && !enumCols.Contains(value))
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, j, "单元格的值不在枚举列表中！");
                        }
                    }
                }
            }

            #endregion

            #region 校验正则表达式

            if (!String.IsNullOrEmpty(detailConfig.Extend1))
            {
                var cols = detailConfig.Cells.Split(',');

                for (var i = this.StartRow; i <= worksheet.LastRowNum; i++)
                {
                    for (var j = this.StartColumn; j < worksheet.GetRow(this.StartRow).LastCellNum; j++)
                    {
                        var value = worksheet.GetRow(i).GetCell(j).GetCellValue();
                        try
                        {
                            //校验正则表达式
                            if (cols.Contains(j.ToString()) && !String.IsNullOrEmpty(value) && !(new Regex(@detailConfig.Extend1)).IsMatch(value))
                            {
                                errorCount++;
                                worksheet.SetCellComment(i, j, "单元格的值不满足正则表达式！");
                            }
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            worksheet.SetCellComment(i, j, ex.Message);
                        }
                    }
                }
            }

            #endregion

            return errorCount;
        }

        /// <summary>
        /// 校验日期
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        /// <param name="detailConfig">配置明细</param>
        /// <returns>错误数量</returns>
        private int DateValidation(ISheet worksheet, ImportConfigDetailDTO detailConfig)
        {
            var errorCount = 0;

            #region 校验日期

            var cols = detailConfig.Cells.Split(',');

            for (var i = this.StartRow; i <= worksheet.LastRowNum; i++)
            {
                for (var j = this.StartColumn; j < worksheet.GetRow(this.StartRow).LastCellNum; j++)
                {
                    var value = worksheet.GetRow(i).GetCell(j).GetCellValue();

                    //校验日期
                    if (cols.Contains(j.ToString()) && !String.IsNullOrEmpty(value) && !Common.Utility.NGDateUtil.IsDate(value))
                    {
                        errorCount++;
                        worksheet.SetCellComment(i, j, "单元格的值不是日期！");
                    }
                }
            }

            #endregion

            return errorCount;
        }

        /// <summary>
        /// 校验时间
        /// </summary>
        /// <param name="worksheet">工作簿</param>
        /// <param name="detailConfig">配置明细</param>
        /// <returns>错误数量</returns>
        private int DateTimeValidation(ISheet worksheet, ImportConfigDetailDTO detailConfig)
        {
            var errorCount = 0;

            #region 校验时间

            var cols = detailConfig.Cells.Split(',');

            for (var i = this.StartRow; i <= worksheet.LastRowNum; i++)
            {
                for (var j = this.StartColumn; j < worksheet.GetRow(this.StartRow).LastCellNum; j++)
                {
                    var value = worksheet.GetRow(i).GetCell(j).GetCellValue();

                    //校验时间
                    if (cols.Contains(j.ToString()) && !String.IsNullOrEmpty(value) && !Common.Utility.NGDateUtil.IsDateTime(value))
                    {
                        errorCount++;
                        worksheet.SetCellComment(i, j, "单元格的值不是时间！");
                    }
                }
            }

            #endregion

            return errorCount;
        }

        #endregion
    }
}
