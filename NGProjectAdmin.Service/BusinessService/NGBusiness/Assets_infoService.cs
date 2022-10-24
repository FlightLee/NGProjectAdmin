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
    public class Assets_infoService : BaseService<Assets_info>, IAssets_infoService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IAssets_infoRepository Assets_infoRepository;

        public Assets_infoService(IAssets_infoRepository Assets_infoRepository) : base(Assets_infoRepository)
        {
            this.Assets_infoRepository = Assets_infoRepository;
        }
        #endregion

    }
}
