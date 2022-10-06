using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessModuleRepository
{
    /// <summary>
    /// BizModule数据访问层实现
    /// </summary>   
    public class BizModuleRepository : NGAdminBaseRepository<BizModule>, IBizModuleRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public BizModuleRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
