

using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.ImportDetailRepository;
using NGProjectAdmin.Service.Base;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.ImportDetailService
{
    /// <summary>
    /// 导入配置明细业务层实现
    /// </summary>
    public class ImportConfigDetailService : NGAdminBaseService<SysImportConfigDetail>, IImportConfigDetailService
    {
        /// <summary>
        /// 导入配置明细访问层实例
        /// </summary>
        private readonly IImportConfigDetailRepository importConfigDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="importConfigDetailRepository"></param>
        public ImportConfigDetailService(IImportConfigDetailRepository importConfigDetailRepository) : base(importConfigDetailRepository)
        {
            this.importConfigDetailRepository = importConfigDetailRepository;
        }
    }
}
