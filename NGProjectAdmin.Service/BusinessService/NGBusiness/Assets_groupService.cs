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
    public class Assets_groupService : BaseService<Assets_group>, IAssets_groupService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IAssets_groupRepository Assets_groupRepository;

        public Assets_groupService(IAssets_groupRepository Assets_groupRepository) : base(Assets_groupRepository)
        {
            this.Assets_groupRepository = Assets_groupRepository;
        }
        #endregion

    }
}
