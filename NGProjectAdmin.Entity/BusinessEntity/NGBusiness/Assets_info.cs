using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessEntity.NGBusiness
{
    /// <summary>
    /// 资产信息档案
    /// </summary>
    [SugarTable("assets_info")]
    public class Assets_info: BaseEntity
    {
    
        /// <summary>
        /// 档案编号
        /// </summary>
        public string? AssetsCode { get; set; }


        /// <summary>
        /// 资产取得时间
        /// </summary>
        public DateTime? AssetsGetDate { get; set; }

        /// <summary>
        /// 资产类型0租赁型住宅1租赁型门面房2土地3经营性用房4商服用房5工厂用房6沿街商铺
        /// </summary>
        public int? AssetsTypeId { get; set; }

        /// <summary>
        /// 资产来源0代管1自购
        /// </summary>
        public int? AssetsSourceId { get; set; }
        /// <summary>
        /// 0闲置中1出租中
        /// </summary>
        public int AssetsState { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public double AssetsArea { get; set; }
        /// <summary>
        /// 固定资产原值
        /// </summary>
        public string? AssetsBefore { get; set; }
        /// <summary>
        /// 固定资产现值
        /// </summary>
        public string? AssetsValue { get; set; }
        /// <summary>
        /// 资产地址
        /// </summary>
        public string? AssetsAdress { get; set; }
        /// <summary>
        /// 地图标注
        /// </summary>
        public string? MapInfo { get; set; }
        /// <summary>
        /// 产权人
        /// </summary>
        public string? PropertyOwner { get; set; }
        /// <summary>
        /// 资产用途描述
        /// </summary>
        public string? AssetsFor { get; set; }
        /// <summary>
        /// 相关资料附件
        /// </summary>
        public string? AssetsFileGroupId { get; set; }

        /// <summary>
        /// 资料附件
        /// </summary>
        [Navigate(NavigateType.OneToOne, nameof(AssetsFileGroupId), nameof(File_group.Id))]
        public File_group? assetsFiles { get; set; }
        /// <summary>
        /// 最新评估表Id
        /// </summary>
        public string? AssetsMentGroupId { get; set; }

        /// <summary>
        /// 资产评估表
        /// </summary>
        [Navigate(NavigateType.OneToOne, nameof(AssetsMentGroupId), nameof(Assetment_group.Id))]
        public Assetment_group? assetment_group { get; set; }
        /// <summary>
        /// 房产证号
        /// </summary>
        public string? PropertyCode { get; set; }
        /// <summary>
        /// 土地证号
        /// </summary>
        public string? landCode { get; set; }
        /// <summary>
        /// 不懂产权证号
        /// </summary>
        public string? LandPropertyInfo { get; set; }
        public string? propertyFileGroupId { get; set; }

        /// <summary>
        /// 产权资料附件
        /// </summary>
        [Navigate(NavigateType.OneToOne, nameof(propertyFileGroupId), nameof(File_group.Id))]
        public File_group? propertyFiles { get; set; }
        /// <summary>
        /// 处置方式0移交1拆迁2出借3停用4正常管理
        /// </summary>
        public int AssetUseType { get; set; }

        /// <summary>
        /// 当前合同Id
        /// </summary>
        public string? ContractCode { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(ContractCode), nameof(Contract_baseinfo.Id))]
        public Contract_baseinfo? contract_baseinfo { get; set; }
        
    }
}
