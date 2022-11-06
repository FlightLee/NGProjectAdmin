using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.NGBusiness
{
    /// <summary>
    /// 资产档案列表
    /// </summary>
    public class Assets_infoDTO: Assets_info
    {
        public Contract_baseinfo Currentcontract { get; set; }
        public List<Contract_baseinfo> listContract { get; set; }
    }
}
