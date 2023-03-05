using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessEntity.NGBusiness
{

    [SugarTable("contract_feeinfo")]
    public class Contract_feeinfo: NGAdminBaseEntity
    {
        public Contract_feeinfo()
        { 
            
        }
        public Contract_feeinfo(IHttpContextAccessor httpContext)
        {
            this.Create(httpContext);
        }
        public string contractId { get; set; } = "";

        public double Amount { get; set; }
        public string OpenId { get; set; } = "";
        public string WchatName { get; set; } = "";
        public DateTime contractBeginTime { get; set; }
        public DateTime contractEndTime { get; set; }
        public DateTime FeeDatetime { get; set; }

        public int IsFee { get; set; }

    }
}
