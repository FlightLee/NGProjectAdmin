using Microsoft.AspNetCore.Http;
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
        public static UserBaseInfo GetCurrentUserInfo(IHttpContextAccessor context)
        {
            var token = context.HttpContext.GetToken();

            var user = RuYiRedisContext.Get<SysUserDTO>(token);

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

            var user = RuYiAdminSessionContext.GetCurrentUserInfo(context);
            if (user.IsSupperAdmin.Equals((int)YesNo.YES) && user.OrgId.Equals(Guid.Empty))
            {
                orgId = RuYiAdminGlobalContext.SystemConfig.OrgRoot;
            }
            else
            {
                orgId = user.OrgId;
            }

            return orgId;
        }
    }
}
