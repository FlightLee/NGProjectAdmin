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
    public class contract_groupService : NGAdminBaseService<contract_group>, Icontract_groupService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly Icontract_groupRepository Contract_groupRepository;

        public contract_groupService(Icontract_groupRepository contract_groupRepository) : base(contract_groupRepository)
        {
            this.Contract_groupRepository = contract_groupRepository;
        }
        #endregion

    }
}
