using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.NGBusiness
{
    public class Assets_info_AssetMentDTO
    {
        public string? AssetMentId { get; set; }

        public string? id { get; set; }
        public DateTime? buildDate { get; set; }

        public double? assessArea { get; set; }

        public string? assetPriceOneYear { get; set; }

        public string? assetCode { get; set; }
    }
}
