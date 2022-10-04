
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.RecordRepository
{
    /// <summary>
    /// SysRecord仓储层实现
    /// </summary>   
    public class SysRecordRepository : NGAdminBaseRepository<SysRecord>, ISysRecordRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public SysRecordRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
