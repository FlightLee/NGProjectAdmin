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
    public class Assetment_groupService : BaseService<Assetment_group>, IAssetment_groupService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IAssetment_groupRepository Assetment_groupRepository;

        public Assetment_groupService(IAssetment_groupRepository Assetment_groupRepository) : base(Assetment_groupRepository)
        {
            this.Assetment_groupRepository = Assetment_groupRepository;
        }
        #endregion

    }
}
