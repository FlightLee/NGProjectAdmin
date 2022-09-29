using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// Elasticsearch配置
    /// </summary>
    public class ElasticsearchConfig
    {
        /// <summary>
        /// Elasticsearch URL
        /// </summary>
        public String Uri { get; set; }

        /// <summary>
        /// 默认索引
        /// </summary>
        public String DefaultIndex { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }
    }
}
