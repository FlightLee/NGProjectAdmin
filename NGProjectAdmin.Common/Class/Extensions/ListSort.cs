using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NGProjectAdmin.Common.Class.Extensions
{
    /// <summary>
    /// list排序扩展
    /// </summary>
    public static class ListSort
    {
        /// <summary>
        /// List排序
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="strSort">排序字段</param>
        /// <returns>list</returns>
        public static List<T> Sort<T>(this List<T> list, String strSort)
        {
            if (!String.IsNullOrEmpty(strSort))
            {
                var arrySorts = strSort.Split(',');
                foreach (var sort in arrySorts)
                {
                    var arr = sort.Split(' ');
                    var field = arr[0];
                    var direction = arr[1];

                    var Queryable = list.AsQueryable();

                    var param = Expression.Parameter(typeof(T), "p");
                    var expression = Expression.Lambda(Expression.Property(param, field), param);

                    if (direction.ToUpper().Equals("ASC"))
                    {
                        list = Queryable.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable), "OrderBy",
                                               new Type[] { Queryable.ElementType, expression.Body.Type },
                                               Queryable.Expression, expression)).ToList();
                    }
                    else if (direction.ToUpper().Equals("DESC"))
                    {
                        list = Queryable.Provider.CreateQuery<T>(Expression.Call(typeof(Queryable), "OrderByDescending",
                                               new Type[] { Queryable.ElementType, expression.Body.Type },
                                               Queryable.Expression, expression)).ToList();
                    }
                }
            }
            return list;
        }
    }
}
