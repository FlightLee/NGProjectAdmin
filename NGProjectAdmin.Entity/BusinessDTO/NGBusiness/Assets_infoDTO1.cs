﻿using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
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
    public class Assets_infoDTO1 : Assets_info
    {
        public Assets_infoDTO1()
        {
            contractinfo = new List<Assets_info_ContractDTO>();
            contractinfo = new List<Assets_info_ContractDTO>();
            assetsMent = new Assets_info_AssetMentDTO();
            assetsFileGroupFiles = new List<File_detail>();
            propertyFileGroupFiles = new List<File_detail>();

        }
        public List<Assets_info_ContractDTO> contractinfo { get; set; }
        public Assets_info_ContractDTO contractinfoMain { get; set; }

        public Assets_info_AssetMentDTO assetsMent { get; set; }

        public List<File_detail> assetsFileGroupFiles { get; set; }

        public List<File_detail> propertyFileGroupFiles { get; set; }


    }
}
