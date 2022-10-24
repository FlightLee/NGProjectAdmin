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
    public class Finance_infoService : BaseService<Finance_info>, IFinance_infoService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IFinance_infoRepository Finance_infoRepository;

        public Finance_infoService(IFinance_infoRepository Finance_infoRepository) : base(Finance_infoRepository)
        {
            this.Finance_infoRepository = Finance_infoRepository;
        }
        #endregion

    }
}
