using Nest;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEnum;
using NGProjectAdmin.Entity.CoreExtensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace NGProjectAdmin.Entity.CoreEntity
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class QueryCondition
    {
        /// <summary>
        /// 起始页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public String Sort { get; set; }

        /// <summary>
        /// 查询项
        /// </summary>
        public List<QueryItem> QueryItems { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public QueryCondition()
        {

        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">分页数量</param>
        /// <param name="sort">排序字段</param>
        /// <param name="queryItems">查询条件</param>
        public QueryCondition(int pageIndex, int pageSize, String sort, List<QueryItem> queryItems)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Sort = sort;
            QueryItems = queryItems;
        }

        #region 公有方法

        /// <summary>
        /// 转化为SQL语句
        /// </summary>
        /// <param name="queryItems">查询条件</param>
        /// <returns>字符串</returns>
        public static String ConvertToSQL(List<QueryItem> queryItems)
        {
            var queryStr = new StringBuilder();

            foreach (var item in queryItems)
            {
                queryStr.Append(ConvertQueryItem(item));
            }

            String result = queryStr.ToString();

            #region 防止JavaScript Sql注入式攻击

            String[] keyWords = { "select", "insert", "delete", "count(", "drop table","drop", "update", "truncate", "asc(", "mid(", "char(", "xp_cmdshell", "exec",
                "master", "net",  "where" };//"and", "or",

            for (int i = 0; i < keyWords.Length; i++)
            {
                if (result.IndexOf(keyWords[i], StringComparison.OrdinalIgnoreCase) > 0)
                {
                    String pattern = String.Format(@"[\W]{0}[\W]", keyWords[i]);
                    Regex rx = new Regex(pattern, RegexOptions.IgnoreCase);
                    if (rx.IsMatch(result))
                    {
                        throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("Warnning:Sql Injection Attack");
                    }
                }
            }

            #endregion

            return ReplaceWarnningItems(result);
        }

        /// <summary>
        /// 过滤非关键字
        /// </summary>
        /// <param name="value">sql</param>
        /// <returns>过滤后的sql</returns>
        public static String ReplaceWarnningItems(String value)
        {
            //value = value.Replace("'", "");
            value = value.Replace(";", "");
            value = value.Replace("--", "");
            value = value.Replace("/**/", "");

            return value;
        }

        /// <summary>
        /// 追加缺省查询条件
        /// </summary>
        /// <param name="queryCondition">QueryCondition对象</param>
        public static void AddDefaultQueryItem(QueryCondition queryCondition)
        {
            if (queryCondition.QueryItems.Count > 0)
            {
                queryCondition.QueryItems.Add(QueryItem.GetDefault());
            }
            else
            {
                queryCondition.QueryItems = new List<QueryItem>();
                queryCondition.QueryItems.Add(QueryItem.GetDefault());
            }
        }

        /// <summary>
        /// 查询条件转Lamda表达式
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="queryItems">查询条件</param>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> BuildExpression<T>(List<QueryItem> queryItems)
        {
            Expression<Func<T, bool>> where = PredicateExtension.True<T>();

            foreach (var queryItem in queryItems)
            {
                var field = queryItem.Field;
                var queryMethod = queryItem.QueryMethod;
                var dataType = queryItem.DataType;
                var value = queryItem.Value;

                if (String.IsNullOrEmpty(field) || value == null)
                {
                    continue;
                }

                Expression constant = null;
                //构建参数
                var parameter = Expression.Parameter(typeof(T), "p");
                //表达式左侧 like: p.Name
                Expression left = Expression.PropertyOrField(parameter, field);
                //表达式右侧，比较值， like '张三'
                Expression right = Expression.Constant(value);

                #region 特殊值处理

                #region 处理Json

                if (right.Type.Equals(typeof(System.Text.Json.JsonElement)))
                {
                    right = Expression.Constant(value.ToString());
                }

                #endregion

                #region 处理操作类型

                if (left.Type.Equals(typeof(OperationType)))
                {
                    right = Expression.Constant((OperationType)int.Parse(value.ToString()), typeof(OperationType));
                }

                #endregion

                #region 处理日期时间

                else if (left.Type.Equals(typeof(DateTime)))
                {
                    right = Expression.Constant(DateTime.Parse(value.ToString()), typeof(DateTime));
                }
                else if (left.Type.Equals(typeof(Nullable<DateTime>)))
                {
                    right = Expression.Constant(DateTime.Parse(value.ToString()), typeof(DateTime?));
                }

                #endregion

                #region 处理Guid

                else if (left.Type.Equals(typeof(Guid)))
                {
                    right = Expression.Constant(Guid.Parse(value.ToString()), typeof(Guid));
                }
                else if (left.Type.Equals(typeof(Guid?)))
                {
                    right = Expression.Constant(Guid.Parse(value.ToString()), typeof(Guid?));
                }

                #endregion

                #region 处理float

                else if (left.Type.Equals(typeof(float)))
                {
                    right = Expression.Constant(float.Parse(value.ToString()), typeof(float));
                }
                else if (left.Type.Equals(typeof(float?)))
                {
                    right = Expression.Constant(float.Parse(value.ToString()), typeof(float?));
                }

                #endregion

                #region 处理Double

                else if (left.Type.Equals(typeof(double)))
                {
                    right = Expression.Constant(double.Parse(value.ToString()), typeof(double));
                }
                else if (left.Type.Equals(typeof(double?)))
                {
                    right = Expression.Constant(double.Parse(value.ToString()), typeof(double?));
                }

                #endregion

                #region 处理int

                else if (left.Type.Equals(typeof(int)))
                {
                    right = Expression.Constant(int.Parse(value.ToString()), typeof(int));
                }
                else if (left.Type.Equals(typeof(int?)))
                {
                    right = Expression.Constant(int.Parse(value.ToString()), typeof(int?));
                }

                #endregion

                #region 处理字符串

                else
                {
                    right = Expression.Constant(value.ToString(), typeof(String));
                }

                #endregion

                #endregion

                switch (queryMethod)
                {
                    case QueryMethod.Equal:
                        constant = Expression.Equal(left, right);
                        break;

                    case QueryMethod.LessThan:
                        constant = Expression.LessThan(left, right);
                        break;

                    case QueryMethod.LessThanOrEqual:
                        constant = Expression.LessThanOrEqual(left, right);
                        break;

                    case QueryMethod.GreaterThan:
                        constant = Expression.GreaterThan(left, right);
                        break;

                    case QueryMethod.GreaterThanOrEqual:
                        constant = Expression.GreaterThanOrEqual(left, right);
                        break;

                    case QueryMethod.BetweenAnd:
                        var arr = value.ToString().Split(',');

                        var lower = dataType == DataType.DateTime ?
                            Expression.Constant(Convert.ToDateTime(arr[0]), typeof(DateTime)) :
                            Expression.Constant(arr[0], typeof(String));

                        var higher = dataType == DataType.DateTime ?
                            Expression.Constant(Convert.ToDateTime(arr[1]), typeof(DateTime)) :
                            Expression.Constant(arr[1], typeof(String));

                        constant = Expression.GreaterThanOrEqual(left, lower);
                        var lambda = Expression.Lambda<Func<T, Boolean>>(constant, parameter);
                        @where = @where.And(lambda);

                        constant = Expression.LessThanOrEqual(left, higher);
                        lambda = Expression.Lambda<Func<T, Boolean>>(constant, parameter);
                        @where = @where.And(lambda);
                        constant = null;
                        break;

                    case QueryMethod.Like:
                    case QueryMethod.Include:
                        var method = dataType == DataType.Int ?
                            typeof(List<int>).GetMethod("Contains", new Type[] { typeof(List<int>) }) :
                            typeof(String).GetMethod("Contains", new Type[] { typeof(String) });

                        constant = Expression.Call(left, method, right);
                        break;

                    case QueryMethod.OrLike:
                        var methodOrLike = dataType == DataType.Int ?
                            typeof(List<int>).GetMethod("Contains", new Type[] { typeof(List<int>) }) :
                            typeof(String).GetMethod("Contains", new Type[] { typeof(String) });

                        constant = Expression.Call(left, methodOrLike, right);
                        lambda = Expression.Lambda<Func<T, Boolean>>(constant, parameter);
                        @where = @where.Or(lambda);

                        constant = null;
                        break;

                    case QueryMethod.NotEqual:
                        constant = Expression.NotEqual(left, right);
                        break;

                    default:
                        break;
                }

                if (constant != null)
                {
                    var lambda = Expression.Lambda<Func<T, Boolean>>(constant, parameter);
                    @where = @where.And(lambda);
                }
            }
            return @where;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns>QueryCondition</returns>
        public static QueryCondition DeserializeObject(string value)
        {
            return JsonConvert.DeserializeObject<QueryCondition>(value);
        }

        /// <summary>
        /// 转化为SearchDescriptor
        /// </summary>
        /// <typeparam name="TDocument">class</typeparam>
        /// <returns>SearchDescriptor</returns>
        public SearchDescriptor<TDocument> ToSearchDescriptor<TDocument>() where TDocument : class
        {
            var searchDescriptor = new SearchDescriptor<TDocument>();

            #region 转化查询条件

            List<QueryItem> queryItems = this.QueryItems;
            if (queryItems != null && queryItems.Count > 0)
            {
                foreach (QueryItem queryItem in queryItems)
                {
                    switch (queryItem.QueryMethod)
                    {
                        case QueryMethod.Equal:
                            if (queryItem.DataType == DataType.Int)
                            {
                                int intValue = int.Parse(queryItem.Value.ToString());
                                searchDescriptor.PostFilter(t => t.Term(x => x.Field(queryItem.Field).Value(intValue)));
                            }
                            else if (queryItem.DataType == DataType.Double)
                            {
                                double doubleValue = double.Parse(queryItem.Value.ToString());
                                searchDescriptor.PostFilter(t => t.Term(x => x.Field(queryItem.Field).Value(doubleValue)));
                            }
                            else
                            {
                                searchDescriptor.PostFilter(t => t.Term(x => x.Field(queryItem.Field).Value(queryItem.Value.ToString())));
                            }
                            break;
                        case QueryMethod.Like:
                            searchDescriptor.PostFilter(t => t.Wildcard(x => x.Field(queryItem.Field).Value("*" + queryItem.Value.ToString() + "*")));
                            break;
                        case QueryMethod.LessThan:
                            searchDescriptor.PostFilter(t => t.Range(x => x.Field(queryItem.Field).LessThan((double?)queryItem.Value)));
                            break;
                        case QueryMethod.LessThanOrEqual:
                            searchDescriptor.PostFilter(t => t.Range(x => x.Field(queryItem.Field).LessThanOrEquals((double?)queryItem.Value)));
                            break;
                        case QueryMethod.GreaterThan:
                            searchDescriptor.PostFilter(t => t.Range(x => x.Field(queryItem.Field).GreaterThan((double?)queryItem.Value)));
                            break;
                        case QueryMethod.GreaterThanOrEqual:
                            searchDescriptor.PostFilter(t => t.Range(x => x.Field(queryItem.Field).GreaterThanOrEquals((double?)queryItem.Value)));
                            break;
                        case QueryMethod.BetweenAnd:
                            Object[] array = queryItem.Value.ToString().Split(",");
                            searchDescriptor.PostFilter(t => t.Range(x => x.Field(queryItem.Field).GreaterThanOrEquals((double?)array[0]).
                            LessThanOrEquals((double?)array[1])));
                            break;
                        case QueryMethod.Include:
                            searchDescriptor.PostFilter(x => x.QueryString(t => t.Fields(f => f.Field(queryItem.Field)).
                            Query(queryItem.Value.ToString().Replace(",", " "))));
                            break;
                        case QueryMethod.OrLike:
                            searchDescriptor.Query(q => q.Bool(b => b.Should(m => m.QueryString(x => x.Fields(t =>
                            t.Field(queryItem.Field)).Query(queryItem.Value.ToString())))));
                            break;
                        case QueryMethod.NotEqual:
                            searchDescriptor.Query(q => q.Bool(b => b.MustNot(m => m.QueryString(x => x.Fields(t =>
                            t.Field(queryItem.Field)).Query(queryItem.Value.ToString())))));
                            break;
                        default:
                            break;
                    }
                }
            }

            #endregion

            #region 转化排序条件

            if (!String.IsNullOrEmpty(this.Sort))
            {
                String column, direction;
                String[] array;
                String[] orders = this.Sort.Split(",");
                foreach (String sort in orders)
                {
                    array = sort.Split(" ");
                    column = array[0];
                    direction = array[1];
                    if (!String.IsNullOrEmpty(direction))
                    {
                        if (direction.Equals("ASC", StringComparison.CurrentCultureIgnoreCase))
                        {
                            searchDescriptor.Sort(t => t.Field(column, SortOrder.Ascending));
                        }
                        else if (direction.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                        {
                            searchDescriptor.Sort(t => t.Field(column, SortOrder.Descending));
                        }
                    }
                }
            }

            #endregion

            //分页
            searchDescriptor.Skip(this.PageIndex * this.PageSize).Take(this.PageSize);

            return searchDescriptor;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 条件转化
        /// </summary>
        /// <param name="queryItem">查询条件</param>
        /// <returns>字符串</returns>
        private static StringBuilder ConvertQueryItem(QueryItem queryItem)
        {
            var condition = new StringBuilder();

            switch (queryItem.QueryMethod)
            {
                case QueryMethod.Equal:
                    if (queryItem.DataType.Equals(DataType.Date))
                    {
                        condition.Append($" and {queryItem.Field}={NGDateUtil.ParseToDate(DateTime.Parse(queryItem.Value.ToString()))}");
                    }
                    else if (queryItem.DataType.Equals(DataType.DateTime))
                    {
                        condition.Append($" and {queryItem.Field}={NGDateUtil.ParseToDateTime(DateTime.Parse(queryItem.Value.ToString()))}");
                    }
                    else
                    {
                        condition.Append($" and {queryItem.Field}='{queryItem.Value}'");
                    }
                    break;

                case QueryMethod.Like:
                    condition.Append($" and {queryItem.Field} like '%{queryItem.Value}%'");
                    break;

                case QueryMethod.LessThan:
                    condition.Append($" and {queryItem.Field}<'{queryItem.Value}'");
                    break;

                case QueryMethod.LessThanOrEqual:
                    condition.Append($" and {queryItem.Field}<='{queryItem.Value}'");
                    break;

                case QueryMethod.GreaterThan:
                    condition.Append($" and {queryItem.Field}>'{queryItem.Value}'");
                    break;

                case QueryMethod.GreaterThanOrEqual:
                    condition.Append($" and {queryItem.Field}>='{queryItem.Value}'");
                    break;

                case QueryMethod.BetweenAnd:
                    var array = queryItem.Value.ToString().Split(',');
                    if (queryItem.DataType.Equals(DataType.Date))
                    {
                        condition.Append($" and {queryItem.Field} between {NGDateUtil.ParseToDate(DateTime.Parse(array[0]))} "
                             + $" and {NGDateUtil.ParseToDate(DateTime.Parse(array[1]))}");
                    }
                    else if (queryItem.DataType.Equals(DataType.DateTime))
                    {
                        condition.Append($" and {queryItem.Field} between {NGDateUtil.ParseToDateTime(DateTime.Parse(array[0]))} "
                            + $" and {NGDateUtil.ParseToDateTime(DateTime.Parse(array[1]))}");
                    }
                    else
                    {
                        condition.Append($" and {queryItem.Field} between '{array[0]}' and '{array[1]}'");
                    }
                    break;

                case QueryMethod.Include:
                    condition.Append($" and {queryItem.Field} in ({queryItem.Value})");
                    break;

                case QueryMethod.OrLike:
                    condition.Append($" or {queryItem.Field} like '%{queryItem.Value}%'");
                    break;

                case QueryMethod.NotEqual:
                    condition.Append($" and {queryItem.Field} <> '{queryItem.Value}'");
                    break;

                default: break;
            }

            return condition;
        }

        #endregion
    }
}
