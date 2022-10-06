using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessUserRepository
{
    /// <summary>
    /// BizUser数据访问层实现
    /// </summary>   
    public class BizUserRepository : NGAdminBaseRepository<BizUser>, IBizUserRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public BizUserRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
