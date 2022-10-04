
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.LanguageRepository
{
    /// <summary>
    /// 多语数据访问层实现
    /// </summary>
    public class LanguageRepository : NGAdminBaseRepository<SysLanguage>, ILanguageRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public LanguageRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
