using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.UserRepository
{
    /// <summary>
    /// 用户数据访问层实现
    /// </summary>
    public class UserRepository : NGAdminBaseRepository<SysUser>, IUserRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
