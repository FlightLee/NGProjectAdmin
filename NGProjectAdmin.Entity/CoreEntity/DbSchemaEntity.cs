using System;
using System.Collections.Generic;

namespace NGProjectAdmin.Entity.CoreEntity
{
    /// <summary>
    /// 实体模型
    /// </summary>
    public class DbSchemaEntity
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public DbSchemaEntity()
        {
            this.Fields = new List<DbSchemaField>();
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="name"></param>
        public DbSchemaEntity(string name) : this()
        {
            this.EntityName = name;
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 字段集
        /// </summary>
        public List<DbSchemaField> Fields { get; set; }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>输出</returns>
        public String ToTitleCase(String input)
        {
            var output = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
            return output;
        }
    }
}
