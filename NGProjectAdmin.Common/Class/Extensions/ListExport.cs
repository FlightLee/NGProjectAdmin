
using NGProjectAdmin.Common.Class.Excel;
using NGProjectAdmin.Common.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NGProjectAdmin.Common.Class.Extensions
{
    /// <summary>
    /// List导出拓展类
    /// </summary>
    public static class ListExport
    {
        /// <summary>
        /// 导出字典集
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>字典集</returns>
        public static List<Dictionary<String, Object>> Export<T>(this List<T> list)
        {
            var dictionaryList = new List<Dictionary<String, Object>>();

            foreach (var item in list)//遍历集合
            {
                var dictionary = new Dictionary<String, Object>();

                var properties = item.GetType().GetProperties();//获得元素属性                
                foreach (var property in properties)//遍历元素的属性
                {
                    if (!property.IsDefined(typeof(ExcelExportAttribute), false))
                    {
                        continue;
                    }

                    var attributes = property.GetCustomAttributes();//获得元素Custom Attribute
                    foreach (var attribute in attributes)//遍历元素的Custom Attribute
                    {
                        if (attribute.GetType().Equals(typeof(ExcelExportAttribute)))
                        {
                            var fieldName = ((ExcelExportAttribute)attribute).FieldName;//获得列名                                                       
                            var fieldValue = item.GetType().GetProperty(property.Name).GetValue(item);//获得列值

                            var textEnum = ((ExcelExportAttribute)attribute).TextEnum;//获得类型枚举
                            if (!String.IsNullOrEmpty(textEnum))
                            {
                                Dictionary<String, String> dic = textEnum.ToDictionary();
                                if (dic.ContainsKey(fieldValue.ToString()))
                                {
                                    fieldValue = dic[fieldValue.ToString()];
                                }
                            }

                            dictionary.Add(fieldName, fieldValue);//添加到字典
                            break;
                        }
                    }
                }

                dictionaryList.Add(dictionary);
            }

            return dictionaryList;
        }
    }
}
