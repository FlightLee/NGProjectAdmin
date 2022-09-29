using NGProjectAdmin.Common.Global;
using System;

namespace NGProjectAdmin.Entity.BusinessDTO.SystemManagement
{
    /// <summary>
    /// 代码生成器DTO
    /// </summary>
    public class CodeGeneratorDTO
    {
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

        /// <summary>
        /// 表数组
        /// </summary>
        public String Tables { get; set; }

        /// <summary>
        /// 布局方式
        /// 0:上下布局
        /// 1:左右布局
        /// </summary>
        public int LayoutMode { get; set; }

        /// <summary>
        /// 自动补全路径
        /// </summary>
        public void AutoFillFullName()
        {
            this.DTONamespace = NGAdminGlobalContext.CodeGeneratorConfig.DTONamespace + this.EntityNamespace;
            this.EntityNamespace = NGAdminGlobalContext.CodeGeneratorConfig.EntityNamespace + this.EntityNamespace;
            this.ControllerNamespace = NGAdminGlobalContext.CodeGeneratorConfig.ControllerNamespace + this.ControllerNamespace;
            this.ServiceNamespace = NGAdminGlobalContext.CodeGeneratorConfig.ServiceNamespace + this.ServiceNamespace;
            this.RepositoryNamespace = NGAdminGlobalContext.CodeGeneratorConfig.RepositoryNamespace + this.RepositoryNamespace;
        }
    }
}
