using System;

namespace NGProjectAdmin.Common.Global
{
    /// <summary>
    /// 代码生成器配置
    /// </summary>
    public class CodeGeneratorConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 忽略字段
        /// </summary>
        public String FieldsIgnoreCase { get; set; }

        /// <summary>
        /// 模型层命名空间
        /// </summary>
        public String EntityNamespace { get; set; }

        /// <summary>
        /// DTO模型命名空间
        /// </summary>
        public String DTONamespace { get; set; }

        /// <summary>
        /// 控制层命名空间
        /// </summary>
        public String ControllerNamespace { get; set; }

        /// <summary>
        /// 服务层命名空间
        /// </summary>
        public String ServiceNamespace { get; set; }

        /// <summary>
        /// 仓储层命名空间
        /// </summary>
        public String RepositoryNamespace { get; set; }
    }
}
