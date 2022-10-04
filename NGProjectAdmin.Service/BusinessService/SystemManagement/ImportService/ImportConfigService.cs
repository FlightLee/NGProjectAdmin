

using AutoMapper;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.ImportDetailRepository;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.ImportRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.ImportService
{
    /// <summary>
    /// 导入配置业务层实现
    /// </summary>
    public class ImportConfigService : NGAdminBaseService<SysImportConfig>, IImportConfigService
    {
        #region 属性及构造函数

        /// <summary>
        /// 导入配置访问层实例
        /// </summary>
        private readonly IImportConfigRepository importConfigRepository;

        /// <summary>
        /// 导入配置明细访问层实例
        /// </summary>
        private readonly IImportConfigDetailRepository importConfigDetailRepository;

        /// <summary>
        /// AutoMapper实例
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="importConfigRepository"></param>
        /// <param name="importConfigDetailRepository"></param>
        /// <param name="mapper"></param>
        public ImportConfigService(IImportConfigRepository importConfigRepository,
            IImportConfigDetailRepository importConfigDetailRepository,
            IMapper mapper) : base(importConfigRepository)
        {
            this.importConfigRepository = importConfigRepository;
            this.importConfigDetailRepository = importConfigDetailRepository;
            this.mapper = mapper;
        }

        #endregion

        #region 获取导入配置

        /// <summary>
        /// 获取导入配置
        /// </summary>
        /// <param name="configName">配置名称</param>
        /// <returns>配置信息</returns>
        public ImportConfigDTO GetImportConfig(String configName)
        {
            var config = this.importConfigRepository.GetList().Where(t => t.IsDel == 0 && t.ConfigName.Equals(configName)).FirstOrDefault();

            var configDetails = this.importConfigDetailRepository.GetList().Where(t => t.IsDel == 0 && t.ParentId.Equals(config.Id)).ToList();

            var configDTO = this.mapper.Map<ImportConfigDTO>(config);

            configDTO.Children = this.mapper.Map<List<ImportConfigDetailDTO>>(configDetails);

            return configDTO;
        }

        #endregion
    }
}
