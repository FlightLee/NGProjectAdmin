using System;
using System.Text.RegularExpressions;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// 数字工具类
    /// </summary>
    public class NGDigitUtil
    {
        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns>bool</returns>
        public static bool IsNumber(Object obj)
        {
            var result = false;

            if (obj != null)
            {
                string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";

                result = Regex.IsMatch(obj.ToString(), pattern);
            }

            return result;
        }

        /// <summary>
        /// 判断是否是整数
        /// </summary>
        /// <param name="obj">输入</param>
        /// <returns>bool</returns>
        public static bool IsInt(Object obj)
        {
            var result = false;

            if (obj != null)
            {
                result = Regex.IsMatch(obj.ToString(), @"^[0-9]\d*$");
            }

            return result;
        }

        /// <summary>
        /// 判断是否为手机号或者座机号
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns>bool</returns>
        public static bool isPhoneNumber(Object obj)
        {
            var result = false;

            if (obj != null)
            {
                Regex regex = new Regex(@"^(0[0-9]{2,3}\-)([2-9][0-9]{6,7})?(\-[0-9]{1,4})?$|(^(13[0-9]|15[0-9]|17[0-9]|18[0-9])\d{8}$)");

                result = regex.IsMatch(obj.ToString());
            }

            return result;
        }
    }
}
