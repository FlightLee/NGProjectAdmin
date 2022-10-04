using NGProjectAdmin.Entity.CoreDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.CodeGeneratorRepository
{
    /// <summary>
    /// 代码生成器数据访问层接口
    /// </summary>
    public interface ICodeGeneratorRepository
    {
        /// <summary>
        /// 获取表名称列表
        /// </summary>
        /// <returns>表名称列表</returns>
        Task<List<DbSchemaInfoDTO>> GetSchemaInfo();

        /// <summary>
        /// 获取表的列表
        /// </summary>
        /// <param name="tables">表名</param>
        /// <returns>表的列表集</returns>
        Task<List<DbSchemaInfoDTO>> GetSchemaFieldsInfo(String tables);
    }
}
