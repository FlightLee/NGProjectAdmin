using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.NGBusiness
{
    public class Assets_info_ContractDTOGroup
    {
        public string year { get; set; }
        public List<Assets_info_ContractDTO> contractinfoGroups { get; set; }
    }
}
