
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.AreaRepository
{
    /// <summary>
    /// 行政区域数据访问层实现
    /// </summary>
    public class AreaRepository : NGAdminBaseRepository<SysArea>, IAreaRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public AreaRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
