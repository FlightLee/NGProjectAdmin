using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessAccountRepository;
using NGProjectAdmin.Service.Base;

namespace NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessAccountService
{
    /// <summary>
    /// BizAccount业务层实现
    /// </summary>
    public class BizAccountService : NGAdminBaseService<BizAccount>, IBizAccountService
    {
        #region 属性及构造函数

        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IBizAccountRepository BizAccountRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BizAccountRepository"></param>
        public BizAccountService(IBizAccountRepository BizAccountRepository) : base(BizAccountRepository)
        {
            this.BizAccountRepository = BizAccountRepository;
        }

        #endregion
    }
}
