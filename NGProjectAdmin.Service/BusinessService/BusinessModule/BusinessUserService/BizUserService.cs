
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessUserRepository;
using NGProjectAdmin.Service.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessUserService
{
    /// <summary>
    /// BizUser业务层实现
    /// </summary>
    public class BizUserService : NGAdminBaseService<BizUser>, IBizUserService
    {
        #region 属性及其构造函数        

        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IBizUserRepository BizUserRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BizUserRepository"></param>
        public BizUserService(IBizUserRepository BizUserRepository) : base(BizUserRepository)
        {
            this.BizUserRepository = BizUserRepository;
        }

        #endregion

        #region 获取业务用户

        /// <summary>
        /// 获取业务用户
        /// </summary>
        /// <param name="moduleShortName">模块短名</param>
        /// <param name="logonName">登陆账号</param>
        /// <returns>业务用户</returns>
        public async Task<BizUser> GetBizUser(String moduleShortName, String logonName)
        {
            var queryCondition = new QueryCondition();
            queryCondition.QueryItems = new List<QueryItem>();
            queryCondition.QueryItems.Add(new QueryItem()
            {
                Field = "USER_LOGON_NAME",
                DataType = Entity.CoreEnum.DataType.String,
                Value = String.Join('_', moduleShortName, logonName),
                QueryMethod = Entity.CoreEnum.QueryMethod.Like
            });

            RefAsync<int> totalCount = 0;
            var user = (await this.BizUserRepository.SqlQueryAsync(queryCondition, "sqls:sql:query_biz_user", totalCount)).FirstOrDefault();

            return user;
        }

        #endregion
    }
}
