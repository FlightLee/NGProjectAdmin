using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Repository.BusinessRepository.NGBusiness;
using NGProjectAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.BusinessService.NGBusiness
{
    public class Contract_defaultService : BaseService<Contract_default>, IContract_defaultService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IContract_defaultRepository Contract_defaultRepository;

        public Contract_defaultService(IContract_defaultRepository Contract_defaultRepository) : base(Contract_defaultRepository)
        {
            this.Contract_defaultRepository = Contract_defaultRepository;
        }
        #endregion

    }
}
