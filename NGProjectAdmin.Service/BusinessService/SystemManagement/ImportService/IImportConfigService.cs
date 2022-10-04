

using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Service.Base;
using System;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.ImportService
{
    /// <summary>
    /// 导入配置业务层接口
    /// </summary>
    public interface IImportConfigService : INGAdminBaseService<SysImportConfig>
    {
        /// <summary>
        /// 获取导入配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>配置信息</returns>
        ImportConfigDTO GetImportConfig(String configName);
    }
}
