
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.ImportRepository
{
    /// <summary>
    /// 导入配置数据访问层实现
    /// </summary>
    public class ImportConfigRepository : NGAdminBaseRepository<SysImportConfig>, IImportConfigRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public ImportConfigRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
