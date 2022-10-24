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
    public class Contract_baseinfoService : BaseService<Contract_baseinfo>, IContract_baseinfoService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IContract_baseinfoRepository Contract_baseinfoRepository;

        public Contract_baseinfoService(IContract_baseinfoRepository Contract_baseinfoRepository) : base(Contract_baseinfoRepository)
        {
            this.Contract_baseinfoRepository = Contract_baseinfoRepository;
        }
        #endregion

    }
}
