using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.File;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessUserRepository
{
    /// <summary>
    /// File_group数据访问层实现
    /// </summary>   
    public class File_groupRepository : NGAdminBaseRepository<File_group>, IFile_groupRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public File_groupRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }
    }
}
