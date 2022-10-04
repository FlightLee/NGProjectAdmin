using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.CoreDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.CodeGeneratorService
{
    /// <summary>
    /// 代码生产服务层接口
    /// </summary>
    public interface ICodeGeneratorService
    {
        /// <summary>
        /// 获取表名称列表
        /// </summary>
        /// <returns>表名称列表</returns>
        Task<List<DbSchemaInfoDTO>> GetSchemaInfo();

        /// <summary>
        /// 自动生成代码
        /// </summary>
        /// <param name="codeGenerator">参数</param>
        /// <returns>zipId</returns>
        Task<Guid> CodeGenerate(CodeGeneratorDTO codeGenerator);
    }
}
