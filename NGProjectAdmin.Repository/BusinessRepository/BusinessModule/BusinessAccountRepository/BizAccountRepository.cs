using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessAccountRepository
{
    /// <summary>
    /// BizAccount数据访问层实现
    /// </summary>   
    public class BizAccountRepository : NGAdminBaseRepository<BizAccount>, IBizAccountRepository
    {
        #region 属性及构造函数

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public BizAccountRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }

        #endregion
    }
}
