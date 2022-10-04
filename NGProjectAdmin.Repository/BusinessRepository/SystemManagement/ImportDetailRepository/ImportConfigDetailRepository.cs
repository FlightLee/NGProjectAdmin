
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.ImportDetailRepository
{
    /// <summary>
    /// 导入配置明细数据访问层实现
    /// </summary>
    public class ImportConfigDetailRepository : NGAdminBaseRepository<SysImportConfigDetail>, IImportConfigDetailRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public ImportConfigDetailRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
