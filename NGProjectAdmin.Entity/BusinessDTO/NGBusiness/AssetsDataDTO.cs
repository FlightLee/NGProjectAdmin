using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessDTO.NGBusiness
{
    public class AssetsDataDTO
    {
        /// <summary>
        /// 资产总数
        /// </summary>
        public int AssetsCount { get; set; } = 0;

        /// <summary>
        /// 当前有效合同
        /// </summary>
        public int validContract { get; set; } = 0;

        /// <summary>
        /// 资产价值
        /// </summary>
        public double invalidContract { get; set; } = 0;

        /// <summary>
        /// 本月到期合同
        /// </summary>
        public int mouthCloseContract { get; set; } = 0;

        public List<string> companyName { get; set; } = new List<string>();

        public List<double> companyPrice { get; set; } = new List<double>();

        public List<string> ytName { get; set; } = new List<string>();

        public List<int> ytCount { get; set; }=new List<int>();

        public DateTime FeeDatetime { get; set; }

        public string lessee { get; set; } = "";

        public string lesseePhone { get; set; } = "";

        public string assetsName { get; set; } = "";

        public double feeAmount { get; set; } = 0;


    }
}
