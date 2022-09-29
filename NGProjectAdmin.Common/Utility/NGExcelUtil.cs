using Nest;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NGProjectAdmin.Common.Class.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// Excel工具类
    /// </summary>
    public static class NGExcelUtil
    {
        #region 导出二进制流

        /// <summary>
        /// 导出二进制流
        /// </summary>
        /// <param name="list">数据集</param>
        /// <returns>流</returns>
        public static Stream ToStream(this List<Dictionary<String, Object>> list)
        {
            //创建工作薄
            var workbook = new HSSFWorkbook();

            //创建表
            var worksheet = workbook.CreateSheet();

            var title = list.FirstOrDefault();

            var row = worksheet.CreateRow(0);
            row.HeightInPoints = 20;

            var index = 0;

            var cell = row.CreateCell(index);
            cell.SetCellValue("序号");

            ICellStyle style = workbook.CreateCellStyle();
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            style.FillPattern = FillPattern.SolidForeground;
            cell.CellStyle = style;

            index++;

            foreach (var item in title)
            {
                cell = row.CreateCell(index);
                cell.SetCellValue(item.Key);

                cell.CellStyle = style;
                cell.CellStyle.WrapText = true;
                cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                cell.CellStyle.Alignment = HorizontalAlignment.Center;

                index++;
            }

            for (var i = 1; i <= list.Count; i++)
            {
                row = worksheet.CreateRow(i);

                index = 0;

                cell = row.CreateCell(index);
                cell.SetCellValue(i);
                cell.CellStyle.WrapText = true;
                cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                cell.CellStyle.Alignment = HorizontalAlignment.Left;

                index++;

                foreach (var item in list[i - 1])
                {
                    cell = row.CreateCell(index);
                    cell.SetCellValue(item.Value != null ? item.Value.ToString() : String.Empty);
                    cell.CellStyle.WrapText = true;

                    index++;
                }
            }

            MemoryStream stream = new MemoryStream();

            //将工作薄写入文件流
            workbook.Write(stream);
            workbook.Close();

            return stream;
        }

        #endregion

        #region 从Excel获取List

        /// <summary>
        /// 从Excel获取List
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="worksheet">工作簿</param>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <param name="dictionary">列表字典</param>
        /// <returns>数据集合</returns>
        public static List<T> ToList<T>(this ISheet worksheet, int startRow, int startColumn, Dictionary<String, String> dictionary) where T : class, new()
        {
            List<T> list = new List<T>();

            int rowCount = worksheet.LastRowNum;//总行数
            if (rowCount >= startRow)
            {
                //映射工作簿列与对象属性之间的关系
                Dictionary<int, String> dicColumn = new Dictionary<int, string>();
                int index = startColumn;
                foreach (var item in dictionary)
                {
                    dicColumn.Add(index, item.Value);
                    index++;
                }

                T obj = new T();
                var properties = obj.GetType().GetProperties().ToList();// 获得此模型的公共属性

                IRow firstRow = worksheet.GetRow(0);//获得第一行
                int cellCount = firstRow.LastCellNum;//列数
                for (int i = startRow; i <= rowCount; i++)
                {
                    obj = new T();

                    IRow currentRow = worksheet.GetRow(i);//第i行
                    for (int j = startColumn; j < cellCount; j++)
                    {
                        object value = null;
                        if (currentRow.GetCell(j) != null)
                        {
                            currentRow.GetCell(j).SetCellType(CellType.String);
                            value = currentRow.GetCell(j).StringCellValue;//取值
                        }
                        else
                        {
                            continue;
                        }

                        //如果非空，则赋给对象的属性
                        if (value != DBNull.Value)
                        {
                            var fieldName = dicColumn[j];//获取第表头的值                                                         
                            foreach (var property in properties) //循环属性
                            {
                                if (property.Name.Equals(fieldName))
                                {
                                    var attributes = property.GetCustomAttributes();//获得元素Custom Attribute
                                    foreach (var attribute in attributes)//遍历元素的Custom Attribute
                                    {
                                        if (attribute.GetType().Equals(typeof(ExcelImportAttribute)))
                                        {
                                            var textEnum = ((ExcelImportAttribute)attribute).TextEnum;//获得类型枚举
                                            if (!String.IsNullOrEmpty(textEnum))
                                            {
                                                Dictionary<String, String> dic = textEnum.ToDictionary();
                                                if (dic.ContainsKey(value.ToString()))
                                                {
                                                    value = dic[value.ToString()];
                                                }
                                            }
                                            break;
                                        }
                                    }

                                    Type propertyType = property.PropertyType;//获取属性类型
                                    if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : Guid.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(int) || propertyType == typeof(int?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : int.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : decimal.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(double) || propertyType == typeof(double?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : double.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : DateTime.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(string))
                                    {
                                        property.SetValue(obj, value, null);
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    list.Add(obj);//对象添加到泛型集合中
                }
            }

            return list;
        }

        #endregion

        #region 从Excel获取List

        /// <summary>
        /// 从Excel获取List
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="worksheet">工作簿</param>
        /// <param name="startRow">起始行</param>
        /// <param name="startColumn">起始列</param>
        /// <returns>数据集合</returns>
        public static List<T> ToList<T>(this ISheet worksheet, int startRow, int startColumn) where T : class, new()
        {
            List<T> list = new List<T>();

            int rowCount = worksheet.LastRowNum;//总行数
            if (rowCount >= startRow)
            {
                T obj = new T();
                var properties = obj.GetType().GetProperties().ToList();// 获得此模型的公共属性

                //映射工作簿列与对象属性之间的关系
                Dictionary<int, String> dicColumn = new Dictionary<int, string>();
                int index = startColumn;
                foreach (var property in properties)//遍历元素的属性
                {
                    if (!property.IsDefined(typeof(ExcelImportAttribute), false))
                    {
                        continue;
                    }

                    var attributes = property.GetCustomAttributes();//获得元素Custom Attribute
                    foreach (var attribute in attributes)//遍历元素的Custom Attribute
                    {
                        if (attribute.GetType().Equals(typeof(ExcelImportAttribute)))
                        {
                            var propertyName = property.Name;//获得列名
                            dicColumn.Add(index, propertyName);
                            index++;
                            break;
                        }
                    }
                }

                IRow firstRow = worksheet.GetRow(0);//获得第一行 
                int cellCount = firstRow.LastCellNum;//列数
                for (int i = startRow; i <= rowCount; i++)
                {
                    obj = new T();

                    IRow currentRow = worksheet.GetRow(i);//第i行
                    for (int j = startColumn; j < cellCount; j++)
                    {
                        object value = null;
                        if (currentRow.GetCell(j) != null)
                        {
                            currentRow.GetCell(j).SetCellType(CellType.String);
                            value = currentRow.GetCell(j).StringCellValue;//取值
                        }
                        else
                        {
                            continue;
                        }

                        //如果非空，则赋给对象的属性
                        if (value != DBNull.Value)
                        {
                            var fieldName = dicColumn[j];//获取第表头的值                                                         
                            foreach (var property in properties) //循环属性
                            {
                                if (property.Name.Equals(fieldName))
                                {
                                    var attributes = property.GetCustomAttributes();//获得元素Custom Attribute
                                    foreach (var attribute in attributes)//遍历元素的Custom Attribute
                                    {
                                        if (attribute.GetType().Equals(typeof(ExcelImportAttribute)))
                                        {
                                            var textEnum = ((ExcelImportAttribute)attribute).TextEnum;//获得类型枚举
                                            if (!String.IsNullOrEmpty(textEnum))
                                            {
                                                Dictionary<String, String> dic = textEnum.ToDictionary();
                                                if (dic.ContainsKey(value.ToString()))
                                                {
                                                    value = dic[value.ToString()];
                                                }
                                            }
                                            break;
                                        }
                                    }

                                    Type propertyType = property.PropertyType;//获取属性类型
                                    if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : Guid.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(int) || propertyType == typeof(int?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : int.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : decimal.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(double) || propertyType == typeof(double?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : double.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                                    {
                                        property.SetValue(obj, String.IsNullOrEmpty(value.ToString()) ? null : DateTime.Parse(value.ToString()), null);
                                    }
                                    else if (propertyType == typeof(string))
                                    {
                                        property.SetValue(obj, value, null);
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    list.Add(obj);//对象添加到泛型集合中
                }
            }

            return list;
        }

        #endregion
    }
}
