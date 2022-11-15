using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.NGBusiness
{
    public class Assets_info_ContractDTO
    {
        public string? contractCode { get; set; }

        public int? contractType { get; set; }
        public string? lessee { get; set; }

        public string? lesseePhone { get; set; }

        public DateTime? ContracStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public double? ContractPrice { get; set; }

        public double? ContractMoney { get; set; }

        public int contractLife { get; set; }

        public double contractPromiseMoney { get; set; }
    }
}
