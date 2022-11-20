using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.NGBusiness
{
    public class Assets_info_ContractDTO
    {
        public string? id { get; set; }

        public int? contractType { get; set; }
        public string? lessee { get; set; }

        public string? lesseePhone { get; set; }

        public string? lessorPhone { get; set; }

        public DateTime? contractDate { get; set; }
        public DateTime? ContracStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public double? ContractPrice { get; set; }

        public double? ContractMoney { get; set; }

        public int? contractLife { get; set; }

        public double? contractPromiseMoney { get; set; }

        public int? contractPayment { get; set; }

        public int? contractState { get; set; }

        public string? remark { get; set; }

        public string? lessor { get; set; }

        public string? lessorId { get; set; }

        public string? lesseeAdress { get; set; }

        public string? lesseeId { get; set; }

        public List<File_detail>? contractPdfGroupFiles { get; set; }

        public string? ContractPdfGroupId { get; set; }
    }
}
