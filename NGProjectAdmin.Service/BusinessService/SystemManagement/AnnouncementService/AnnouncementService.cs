
using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.SystemManagement.AnnouncementRepository;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.AnnouncementService
{
    /// <summary>
    /// 通知公告服务层实现
    /// </summary>
    public class AnnouncementService : NGAdminBaseService<SysAnnouncement>, IAnnouncementService
    {
        #region 属性及构造函数

        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IAnnouncementRepository announcementRepository;

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="announcementRepository"></param>
        /// <param name="context"></param>
        public AnnouncementService(IAnnouncementRepository announcementRepository,
                                   IHttpContextAccessor context) : base(announcementRepository)
        {
            this.announcementRepository = announcementRepository;
            this.context = context;
        }

        #endregion

        #region 查询通知列表

        /// <summary>
        /// 查询通知列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql key</param>
        /// <returns>QueryResult</returns>
        public async Task<QueryResult<SysNotificationDTO>> QueryNotifications(QueryCondition queryCondition, string sqlKey)
        {
            var strSQL = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;
            //获取用户信息
            var user = NGAdminSessionContext.GetCurrentUserInfo(this.context);
            strSQL = String.Format(strSQL, user.Id);

            int totalCount = 0;
            List<SysNotificationDTO> list = await this.announcementRepository.SqlQueryAsync<SysNotificationDTO>(queryCondition, totalCount, strSQL);

            return QueryResult<SysNotificationDTO>.Success(totalCount, list);
        }

        #endregion
    }
}
