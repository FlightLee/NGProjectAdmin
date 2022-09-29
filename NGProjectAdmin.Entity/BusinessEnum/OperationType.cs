
namespace NGProjectAdmin.Entity.BusinessEnum
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        QueryList,

        /// <summary>
        /// 查询实体
        /// </summary>
        QueryEntity,

        /// <summary>
        /// 新增实体
        /// </summary>
        AddEntity,

        /// <summary>
        /// 编辑实体
        /// </summary>
        EditEntity,

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        DeleteEntity,

        /// <summary>
        /// 物理删除实体
        /// </summary>
        RemoveEntity,

        /// <summary>
        /// 上传文件
        /// </summary>
        UploadFile,

        /// <summary>
        /// 下载文件
        /// </summary>
        DownloadFile,

        /// <summary>
        /// 导入数据
        /// </summary>
        ImportData,

        /// <summary>
        /// 导出数据
        /// </summary>
        ExportData,

        /// <summary>
        /// 菜单授权
        /// </summary>
        MenuAuthorization,

        /// <summary>
        /// 用户授权
        /// </summary>
        PermissionAuthorization,

        /// <summary>
        /// 打印
        /// </summary>
        Print,

        /// <summary>
        /// 登录
        /// </summary>
        Logon,

        /// <summary>
        /// 登出
        /// </summary>
        Logout,

        /// <summary>
        /// 强制登出
        /// </summary>
        ForceLogout,

        /// <summary>
        /// 更新密码
        /// </summary>
        UpdatePassword,

        /// <summary>
        /// 启动计划任务
        /// </summary>
        StartScheduleJob,

        /// <summary>
        /// 暂停计划任务
        /// </summary>
        PauseScheduleJob,

        /// <summary>
        /// 恢复计划任务
        /// </summary>
        ResumeScheduleJob,

        /// <summary>
        /// 权限下放
        /// </summary>
        DelegatePermission,

        /// <summary>
        /// 生成代码
        /// </summary>
        GenerateCode,

        /// <summary>
        /// Sql注入攻击
        /// </summary>
        SqlInjectionAttack,

        /// <summary>
        /// Token劫持
        /// </summary>
        TokenHijacked,

        /// <summary>
        /// 统一认证
        /// </summary>
        UnifiedAuthentication,

        /// <summary>
        /// 统一认证授权
        /// </summary>
        UnifiedAuthorization,

        /// <summary>
        /// 解除统一授权
        /// </summary>
        RemoveUnifiedAuthorization,

        /// <summary>
        /// 获取同步口令
        /// </summary>
        GetSyncToken,

        /// <summary>
        /// 同步新增用户
        /// </summary>
        SyncAddUser,

        /// <summary>
        /// 同步编辑用户
        /// </summary>
        SyncEditUser,

        /// <summary>
        /// 同步删除用户
        /// </summary>
        SyncDeleteUser
    }
}
