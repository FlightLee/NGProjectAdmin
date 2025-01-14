﻿using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
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
    public class Rent_detailService : BaseService<Rent_detail>, IRent_detailService
    {
        #region 属性及其构造函数   
        /// <summary>
        /// 仓储实例
        /// </summary>
        private readonly IRent_detailRepository Rent_detailRepository;

        public Rent_detailService(IRent_detailRepository Rent_detailRepository) : base(Rent_detailRepository)
        {
            this.Rent_detailRepository = Rent_detailRepository;
        }
        #endregion

    }
}
