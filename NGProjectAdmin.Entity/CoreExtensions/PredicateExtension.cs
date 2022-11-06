
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NGProjectAdmin.Entity.CoreExtensions
{
    /// <summary>
    /// 构建lambda表达式范例
    /// </summary>
    public static class PredicateExtension
    {
        /// <summary>
        /// lambda表达式：p=>true
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return a => true;
        }

        /// <summary>
        /// lambda表达式：p=>false
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return p => false;
        }

        /// <summary>
        /// lambda表达式：p=>p.propertyName
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <typeparam name="TKey">数据类型</typeparam>
        /// <param name="propertyName">字段</param>
        /// <returns></returns>
        public static Expression<Func<T, TKey>> GetOrderExpression<T, TKey>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
        }

        /// <summary>
        /// lambda表达式：p=>p.propertyName == propertyValue
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="propertyName">字段</param>
        /// <param name="propertyValue">值</param>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> CreateEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
        }

        /// <summary>
        /// lambda表达式：p=>p.propertyName != propertyValue
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="propertyName">字段</param>
        /// <param name="propertyValue">值</param>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> CreateNotEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
        }

        /// <summary>
        /// lambda表达式：p=>p.propertyName > propertyValue
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="propertyName">字段</param>
        /// <param name="propertyValue">值</param>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> CreateGreaterThan<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
        }

        /// <summary>
        /// lambda表达式：p=>p.propertyName<propertyValue
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="propertyName">字段</param>
        /// <param name="propertyValue">值</param>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> CreateLessThan<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数          
            return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
        }

        /// <summary>
        /// lambda表达式：p=>p.propertyName >= propertyValue
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="propertyName">字段</param>
        /// <param name="propertyValue">值</param>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> CreateGreaterThanOrEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// lambda表达式：p=>p.propertyName<=propertyValue
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="propertyName">字段</param>
        /// <param name="propertyValue">值</param>
        /// <returns>表达式</returns>
        public static Expression<Func<T, bool>> CreateLessThanOrEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// lambda表达式：p=>p.propertyName.Contains(propertyValue)
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="propertyName">字段</param>
        /// <param name="propertyValue">值</param>
        /// <returns>表达式</returns>
        private static Expression<Func<T, bool>> GetContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }

        /// <summary>
        /// lambda表达式：(p=>!p.propertyName.Contains(propertyValue))
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="propertyName">字段</param>
        /// <param name="propertyValue">值</param>
        /// <returns>表达式</returns>
        private static Expression<Func<T, bool>> GetNotContains<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, method, constant)), parameter);
        }

        /// <summary>
        /// 组合And
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="first">第一个表达式</param>
        /// <param name="second">第二个表达式</param>
        /// <returns>组合表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// 组合Or
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="first">第一个表达式</param>
        /// <param name="second">第二个表达式</param>
        /// <returns>组合表达式</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
            .Select((f, i) => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// ParameterRebinder
        /// </summary>
        private class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// The ParameterExpression map
            /// </summary>
            readonly Dictionary<ParameterExpression, ParameterExpression> map;
            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            /// <summary>
            /// Replaces the parameters.
            /// </summary>
            /// <param name="map">The map.</param>
            /// <param name="exp">The exp.</param>
            /// <returns>Expression</returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            /// <summary>
            /// Visits the parameter.
            /// </summary>
            /// <param name="p">The p.</param>
            /// <returns>Expression</returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }
    }
}
