
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.Base;
using System;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.SystemManagement.UserService
{
    /// <summary>
    /// 用户业务层接口
    /// </summary>
    public interface IUserService : INGAdminBaseService<SysUser>
    {
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> DeleteEntity(Guid userId);

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> DeleteEntities(String ids);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginDTO">登录信息</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> Logon(LoginDTO loginDTO);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="operationType">退出类型</param>
        /// <param name="remark">说明信息</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> Logout(String token, OperationType operationType = OperationType.Logout, String remark = null);

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="data">参数</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> UpdatePassword(PasswordDTO data);

        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <returns>QueryResult</returns>
        Task<QueryResult<SysUserDTO>> GetOnlineUsers();

        /// <summary>
        /// 加载系统用户缓存
        /// </summary>
        Task LoadSystemUserCache();

        /// <summary>
        /// 清理系统用户缓存
        /// </summary>
        Task ClearSystemUserCache();
    }
}
