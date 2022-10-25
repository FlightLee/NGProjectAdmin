using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.NGBusiness
{
    public class Assetment_groupDTO:Assetment_group
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
