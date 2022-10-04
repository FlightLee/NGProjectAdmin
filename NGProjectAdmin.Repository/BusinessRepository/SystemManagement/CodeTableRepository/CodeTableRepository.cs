
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.CodeTableRepository
{
    /// <summary>
    /// 数据字典访问层实现
    /// </summary>
    public class CodeTableRepository : NGAdminBaseRepository<SysCodeTable>, ICodeTableRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public CodeTableRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
