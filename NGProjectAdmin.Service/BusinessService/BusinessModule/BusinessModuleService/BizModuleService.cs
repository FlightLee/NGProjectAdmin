using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Repository.BusinessRepository.BusinessModule.BusinessModuleRepository;
using NGProjectAdmin.Service.Base;

namespace NGProjectAdmin.Service.BusinessService.BusinessModule.BusinessModuleService
{
    /// <summary>
    /// BizModule业务层实现
    /// </summary>
    public class BizModuleService : NGAdminBaseService<BizModule>, IBizModuleService
    {
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IBizModuleRepository BizModuleRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="BizModuleRepository"></param>
        public BizModuleService(IBizModuleRepository BizModuleRepository) : base(BizModuleRepository)
        {
            this.BizModuleRepository = BizModuleRepository;
        }
    }
}
