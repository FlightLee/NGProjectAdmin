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
        /// 逾期合同
        /// </summary>
        public int invalidContract { get; set; } = 0;

        /// <summary>
        /// 本月到期合同
        /// </summary>
        public int mouthCloseContract { get; set; } = 0;



    }
}
