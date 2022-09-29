using NGProjectAdmin.Common.Global;
using System;
using System.Text.RegularExpressions;

namespace NGProjectAdmin.Common.Utility
{
    /// <summary>
    /// Date工具类
    /// </summary>
    public class NGDateUtil
    {
        #region 公有方法

        /// <summary>
        /// 转化为日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>字符串</returns>
        public static String ParseToDate(DateTime date)
        {
            var result = String.Empty;

            //MySql = 0,SqlServer = 1,Sqlite = 2,Oracle = 3,PostgreSQL = 4,Dm = 5,Kdbndp = 6
            var type = NGAdminGlobalContext.DBConfig.DBType;

            switch (type)
            {
                case 0:
                    result = ParseToMySqlDate(date);
                    break;
                case 1:
                    result = ParseToSqlServerDate(date);
                    break;
                case 2:
                    //Complete when use
                    break;
                case 3:
                    result = ParseToOracleDate(date);
                    break;
                case 4:
                    result = ParseToPostgreSQLDate(date);
                    break;
                case 5:
                    //Complete when use
                    break;
                case 6:
                    //Complete when use
                    break;
                default: break;
            }
            return result;
        }

        /// <summary>
        /// 转化为时间
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns>字符串</returns>
        public static String ParseToDateTime(DateTime datetime)
        {
            var result = String.Empty;

            //MySql = 0,SqlServer = 1,Sqlite = 2,Oracle = 3,PostgreSQL = 4,Dm = 5,Kdbndp = 6
            var type = NGAdminGlobalContext.DBConfig.DBType;

            switch (type)
            {
                case 0:
                    result = ParseToMySqlDateTime(datetime);
                    break;
                case 1:
                    result = ParseToSqlServerDateTime(datetime);
                    break;
                case 2:
                    //Complete when use
                    break;
                case 3:
                    result = ParseToOracleDateTime(datetime);
                    break;
                case 4:
                    result = ParseToPostgreSQLDateTime(datetime);
                    break;
                case 5:
                    //Complete when use
                    break;
                case 6:
                    //Complete when use
                    break;
                default: break;
            }
            return result;
        }

        /// <summary>
        /// 判断是否为日期
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>bool</returns>
        public static bool IsDate(Object obj)
        {
            var result = false;

            if (obj != null)
            {
                result = Regex.IsMatch(obj.ToString(), @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
            }

            return result;
        }

        /// <summary>
        /// 判断是否为时间
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns>bool</returns>
        public static bool IsTime(Object obj)
        {
            var result = false;

            if (obj != null)
            {
                result = Regex.IsMatch(obj.ToString(), @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
            }

            return result;
        }

        /// <summary>
        /// 判断是否为日期+时间
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns>bool</returns>
        public static bool IsDateTime(Object obj)
        {
            var result = false;

            if (obj != null)
            {
                result = Regex.IsMatch(obj.ToString(), @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
            }

            return result;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 转化为MySQL日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>字符串</returns>
        private static String ParseToMySqlDate(DateTime date)
        {
            return new String($"STR_TO_DATE(‘{date}’,’%Y-%m-%d’)");
        }

        /// <summary>
        /// 转化为MySQL时间
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns>字符串</returns>
        private static String ParseToMySqlDateTime(DateTime datetime)
        {
            return new String($"STR_TO_DATE(‘{datetime}’,’%Y-%m-%d %H:%i:%s:%fff’)");
        }

        /// <summary>
        /// 转化为SqlServer日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>字符串</returns>
        private static String ParseToSqlServerDate(DateTime date)
        {
            return new String($"CAST(N'{date}' AS Date)");
        }

        /// <summary>
        /// 转化为SqlServer时间
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns>字符串</returns>
        private static String ParseToSqlServerDateTime(DateTime datetime)
        {
            return new String($"CAST(N'{datetime}' AS DateTime)");
        }


        /// <summary>
        /// 转化为Oracle日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>字符串</returns>
        private static String ParseToOracleDate(DateTime date)
        {
            return new String($"to_date('{date}','yyyy-mm-dd')");
        }

        /// <summary>
        /// 转化为Oracle时间
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns>字符串</returns>
        private static String ParseToOracleDateTime(DateTime datetime)
        {
            return new String($"to_timestamp('{datetime}','yyyy-mm-dd hh24:mi:ss.ff')");
        }

        /// <summary>
        /// 转化为PostgreSQL日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>字符串</returns>
        private static String ParseToPostgreSQLDate(DateTime date)
        {
            return new String($"to_date('{date}','YYYY-MM-DD')");
        }

        /// <summary>
        /// 转化为PostgreSQL时间
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns>字符串</returns>
        private static String ParseToPostgreSQLDateTime(DateTime datetime)
        {
            return new String($"to_timestamp('{datetime}', 'YYYY-MM-DD HH24:MI:SS') AT TIME ZONE 'CST'");
        }

        #endregion
    }
}
