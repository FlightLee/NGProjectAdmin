//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Repository.Base;

namespace NGProjectAdmin.Repository.BusinessRepository.SystemManagement.AttachmentRepository
{
    /// <summary>
    /// 业务附件数据访问层实现
    /// </summary>
    public class AttachmentRepository : NGAdminBaseRepository<SysAttachment>, IAttachmentRepository
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
        public AttachmentRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }

        #endregion
    }
}
