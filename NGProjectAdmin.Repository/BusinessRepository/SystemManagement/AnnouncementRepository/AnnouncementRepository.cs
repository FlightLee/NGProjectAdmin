using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.AnnouncementRepository
{
    /// <summary>
    /// 通知公告数据访问层实现
    /// </summary>   
    public class AnnouncementRepository : NGAdminBaseRepository<SysAnnouncement>, IAnnouncementRepository
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
        public AnnouncementRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }

        #endregion
    }
}
