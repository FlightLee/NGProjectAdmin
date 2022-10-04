using NGProjectAdmin.Entity.CoreEnum;
using System;

namespace NGProjectAdmin.Entity.CoreEntity
{
    /// <summary>
    /// 查询项
    /// </summary>
    public class QueryItem
    {
        /// <summary>
        /// 查询项字段
        /// </summary>
        public String Field { get; set; }

        /// <summary>
        /// 查询项类型
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// 查询项方法
        /// </summary>
        public QueryMethod QueryMethod { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public Object Value { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public QueryItem()
        {

        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="queryMethod">查询方法</param>
        /// <param name="value">值</param>
        public QueryItem(String field, DataType dataType, QueryMethod queryMethod, Object value)
        {
            Field = field;
            DataType = dataType;
            QueryMethod = queryMethod;
            Value = value;
        }

        /// <summary>
        /// 获取缺省QueryItem
        /// </summary>
        /// <returns>QueryItem</returns>
        public static QueryItem GetDefault()
        {
            return new QueryItem() { Field = "IsDel", DataType = DataType.Int, QueryMethod = QueryMethod.Equal, Value = 0 };
        }
    }
}
