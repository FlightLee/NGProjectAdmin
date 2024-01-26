using AutoMapper;
using Masuit.Tools.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nest;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity;
using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using NGProjectAdmin.Service.Base;
using NGProjectAdmin.Service.BusinessService.NGBusiness;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.WebApi.Controllers.NGBusiness
{
    /// <summary>
    /// 合同相关
    /// </summary>
    public class ContractController : BaseController<Contract_baseinfo>
    {

        #region 属性及构造函数

        /// <summary>
        /// 业务模块接口实例
        /// </summary>
        private readonly IFile_groupService File_groupService;

        private readonly IFile_detailService File_detailService;

        private readonly IContract_baseinfoService Contract_baseinfoService;

        private readonly IAssets_infoService Assets_infoService;

        private readonly IMapper Mapper;
        /// <summary>
        /// 合同控制器
        /// </summary>
        /// <param name="File_groupService"></param>      
        /// <param name="file_detailService"></param>
        /// <param name="contract_baseinfoService"></param>      
        public ContractController(IContract_baseinfoService contract_baseinfoService, IFile_groupService File_groupService, IFile_detailService file_detailService, IAssets_infoService assets_infoService, IMapper mapper) : base(contract_baseinfoService)
        {
            this.File_groupService = File_groupService;
            File_detailService = file_detailService;
            Contract_baseinfoService = contract_baseinfoService;
            Assets_infoService = assets_infoService;
            Mapper = mapper;
        }
        /// <summary>
        /// 删除资产信息
        /// </summary>        
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.DeleteEntity)]
        [Permission("asset:delete:entity")]
        public async Task<IActionResult> DeleteById([FromBody] Contract_baseinfoDTO baseinfo)
        {
            var actionResult = await  Contract_baseinfoService.DeleteAsync(baseinfo.Id);
            //if (Convert.ToBoolean(actionResult.Object))
            //{
            //    actionResult = await Assets_infoService.UpdateAssetsByContractId(baseinfo.Id);

            //}                        
            return Ok(actionResult);

        }

        /// <summary>
        /// 获取全部合同信息
        /// </summary>

        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllContract(QueryCondition queryCondition)
        {
            var actionResult = await Contract_baseinfoService.GetContracts(queryCondition);
         

            return Ok(actionResult);

        }

        #endregion
        /// <summary>
        /// 查询文件信息
        /// </summary>
        /// <param name="FileId">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(string FileId)
        {
            var actionResult = await this.File_groupService.GetByIdAsync(FileId);
            var actionResult2 = this.File_groupService.Add(new File_group(), true);
            //var actionResult2 = NGAdminDbScope.NGDbContext.Queryable<File_group>().Where(x => x.Id == FileId).ToList();
            //var actionResult = await this.userService.Logon(loginDTO);
            return Ok(actionResult);
        }
        /// <summary>
        /// 查询合同信息
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryEntity)]
        [Permission("asset:edit:entity")]
        public async Task<IActionResult> GetById([FromBody] Contract_baseinfoDTO assets_infoDTO)
        {
            var actionResult = await this.Contract_baseinfoService.GetById(assets_infoDTO);
            

            return Ok(actionResult);
        }

        /// <summary>
        /// 查询资产信息
        /// </summary>
        /// <returns>ActionResult</returns>
        public async Task<IActionResult> GetAllAssets()
        {
            var actionResult = await this.Contract_baseinfoService.GetAllAssets();
            return Ok(actionResult);
        }

        /// <summary>
        /// 更新合同信息
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.QueryEntity)]
        [Permission("asset:edit:entity")]
        public async Task<IActionResult> UpdateById([FromBody] Contract_baseinfoDTO contract_Baseinfo)
        {
            if (contract_Baseinfo == null) { return ValidationProblem("合同信息不能为空"); }
            Contract_baseinfo contact = new Contract_baseinfo() { ContractPromiseMoney=Convert.ToDouble(contract_Baseinfo.ContractPromiseMoney) , AssetsId = contract_Baseinfo.AssetsId, ContractPdfGroupId = contract_Baseinfo.ContractPdfGroupId, Id = contract_Baseinfo.Id, ContractDate = contract_Baseinfo.ContractDate, ContractType = contract_Baseinfo.ContractType, Lessor = contract_Baseinfo.Lessor, LessorAdress = contract_Baseinfo.LessorAdress, lessee = contract_Baseinfo.lessee, lesseeId = contract_Baseinfo.lesseeId, lesseePhone = contract_Baseinfo.lesseePhone, lesseeAdress = contract_Baseinfo.lesseeAdress, ContractLife = Convert.ToInt32(contract_Baseinfo.ContractLife), ContracStartDate = contract_Baseinfo.ContracStartDate, ContractEndDate = contract_Baseinfo.ContractEndDate, ContractPrice = Convert.ToDouble(contract_Baseinfo.ContractPrice), ContractMoney = Convert.ToDouble(contract_Baseinfo.ContractMoney), ContractPayment = contract_Baseinfo.ContractPayment, Remark = contract_Baseinfo.Remark, lessorPhone = contract_Baseinfo.lessorPhone, ContractState = contract_Baseinfo.ContractState, lessorId = contract_Baseinfo.lessorId };
            var actionResult = await this.Contract_baseinfoService.UpdateAsync(contact);


            if (contract_Baseinfo.contractPdfGroupFiles != null)
            {
                foreach (var item in contract_Baseinfo.contractPdfGroupFiles)
                {
                    File_detail file_Detail = File_detailService.GetById(item.Id).Object as File_detail;
                    if (file_Detail != null)
                    {
                        file_Detail.FileId = contract_Baseinfo.ContractPdfGroupId;
                        await File_detailService.UpdateAsync(file_Detail);
                    }

                }
            }
            return Ok(actionResult);
        }
       /// <summary>
       /// 新增合同
       /// </summary>
       /// <param name="contract_Baseinfo"></param>
       /// <returns></returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        //[Permission("assetinfo:add:entity")]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] Contract_baseinfoDTO contract_Baseinfo)
        {
            if (contract_Baseinfo == null) { return ValidationProblem("合同信息不能为空"); }


            Contract_baseinfo contact = new Contract_baseinfo() { ContractPromiseMoney = Convert.ToDouble(contract_Baseinfo.ContractPromiseMoney) , AssetsId = contract_Baseinfo.AssetsId, ContractPdfGroupId = contract_Baseinfo.ContractPdfGroupId, Id = contract_Baseinfo.Id, ContractDate = contract_Baseinfo.ContractDate, ContractType = contract_Baseinfo.ContractType, Lessor = contract_Baseinfo.Lessor, LessorAdress = contract_Baseinfo.LessorAdress, lessee = contract_Baseinfo.lessee, lesseeId = contract_Baseinfo.lesseeId, lesseePhone = contract_Baseinfo.lesseePhone, lesseeAdress = contract_Baseinfo.lesseeAdress, ContractLife = Convert.ToInt32(contract_Baseinfo.ContractLife) , ContracStartDate = contract_Baseinfo.ContracStartDate, ContractEndDate = contract_Baseinfo.ContractEndDate, ContractPrice = Convert.ToDouble(contract_Baseinfo.ContractPrice) , ContractMoney = Convert.ToDouble(contract_Baseinfo.ContractMoney), ContractPayment = contract_Baseinfo.ContractPayment, Remark = contract_Baseinfo.Remark, lessorPhone = contract_Baseinfo.lessorPhone, ContractState = contract_Baseinfo.ContractState, lessorId = contract_Baseinfo.lessorId };


            Assets_info assets_Info = Assets_infoService.GetById(contact.AssetsId).Object as Assets_info;
            if (contact.ContractPrice > 0)
            {
                assets_Info.AssetsState = 1;
            }
            contact.AssetsId = contract_Baseinfo.AssetsId;

            await Contract_baseinfoService.AddAsync(contact, true);

            await Contract_baseinfoService.BuildContractFeeInfo(contact);


            var actionResult =await Assets_infoService.UpdateAsync(assets_Info);



            return Ok(actionResult);
        }
    }
}
