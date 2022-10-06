using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessUserModuleRepository
{
    /// <summary>
    /// BizUserModule数据访问层实现
    /// </summary>   
    public class BizUserModuleRepository : NGAdminBaseRepository<BizUserModule>, IBizUserModuleRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public BizUserModuleRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
