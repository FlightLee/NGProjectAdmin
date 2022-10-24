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
    public class Rent_groupService : BaseService<Rent_group>, IRent_groupService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IRent_groupRepository Rent_groupRepository;

        public Rent_groupService(IRent_groupRepository Rent_groupRepository) : base(Rent_groupRepository)
        {
            this.Rent_groupRepository = Rent_groupRepository;
        }
        #endregion

    }
}
