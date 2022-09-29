using System;
using System.Collections.Generic;
using System.Linq;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// 字符串工具类
    /// </summary>
    public static class NGStringUtil
    {
        /// <summary>
        /// 数组字符串转为Guid数组
        /// </summary>
        /// <param name="ids">数组字符串</param>
        /// <returns>Guid数组</returns>
        public static Guid[] GetGuids(String ids)
        {
            if (!String.IsNullOrEmpty(ids))
            {
                var arrStrs = ids.Split(',');

                var arrGuids = new Guid[arrStrs.Length];

                for (var i = 0; i < arrStrs.Length; i++)
                {
                    arrGuids[i] = Guid.Parse(arrStrs[i]);
                }

                return arrGuids;
            }

            return null;
        }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns>bool</returns>
        public static bool IsNullOrEmpty(Object obj)
        {
            var result = false;

            if (obj == null)
            {
                result = true;
            }
            else
            {
                if (String.IsNullOrEmpty(obj.ToString()))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 判断是否包含字符串
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="obj">Object对象</param>
        /// <returns>bool</returns>
        public static bool IsContains(String[] container, Object obj)
        {
            var result = false;

            if (container.Length > 0 && obj != null)
            {
                if (container.Contains(obj.ToString()))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 类型枚举转为字典
        /// </summary>
        /// <param name="textEnum">类型枚举</param>
        /// <returns>字典</returns>
        public static Dictionary<String, String> ToDictionary(this String textEnum)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();

            if (!String.IsNullOrEmpty(textEnum))
            {
                var array = textEnum.Split(",");
                foreach (var text in array)
                {
                    var subArray = text.Split(":");
                    result.Add(subArray[0].ToString(), subArray[1].ToString());
                }
            }

            return result;
        }
    }
}
