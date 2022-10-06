using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessUserModuleService
{
    /// <summary>
    /// BizUserModule业务层接口
    /// </summary>   
    public interface IBizUserModuleService : INGAdminBaseService<BizUserModule>
    {
        /// <summary>
        /// 获取用户模块
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moduleId"></param>
        /// <returns>用户模块对象</returns>
        Task<BizUserModule> GetBizUserModule(Guid userId, Guid moduleId);

        /// <summary>
        /// 统一认证登录
        /// </summary>
        /// <param name="loginDTO">登录信息</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> Logon(LoginDTO loginDTO);

        /// <summary>
        /// 获取同步token
        /// </summary>
        /// <param name="loginDTO">登录信息</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> GetToken(LoginDTO loginDTO);
    }
}
