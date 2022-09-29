using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.CoreEntity
{
    public class NGAdminSessionContext
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>用户信息</returns>
        public static SysUserDTO GetCurrentUserInfo(IHttpContextAccessor context)
        {
            var token = context.HttpContext.GetToken();

            var user = NGRedisContext.Get<SysUserDTO>(token);

            return user;
        }

        /// <summary>
        /// 获取用户机构编号
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>用户机构编号</returns>
        public static Guid GetUserOrgId(IHttpContextAccessor context)
        {
            var orgId = Guid.Empty;

            var user =NGAdminSessionContext.GetCurrentUserInfo(context);
            if (user.IsSupperAdmin.Equals((int)YesNo.YES) && user.OrgId.Equals(Guid.Empty))
            {
                orgId = NGAdminGlobalContext.SystemConfig.OrgRoot;
            }
            else
            {
                orgId = user.OrgId;
            }

            return orgId;
        }
    }
}
