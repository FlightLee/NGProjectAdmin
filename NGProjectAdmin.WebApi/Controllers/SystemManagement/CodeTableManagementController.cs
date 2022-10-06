//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Service.BusinessService.RedisService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.CodeTableService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 数据字典管理控制器
    /// </summary>
    public class CodeTableManagementController : NGAdminBaseController<SysCodeTable>
    {
        #region 属性及构造函数

        /// <summary>
        /// 数据字典接口实例
        /// </summary>
        private readonly ICodeTableService codeTableService;

        /// <summary>
        /// Redis接口实例
        /// </summary>
        private readonly IRedisService redisService;

        /// <summary>
        /// AutoMapper实例
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codeTableService"></param>
        /// <param name="redisService"></param>
        /// <param name="mapper"></param>
        public CodeTableManagementController(ICodeTableService codeTableService,
                                             IRedisService redisService,
                                             IMapper mapper) : base(codeTableService)
        {
            this.codeTableService = codeTableService;
            this.redisService = redisService;
            this.mapper = mapper;
        }

        #endregion

        #region 查询字典列表

        /// <summary>
        /// 查询字典列表
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("code:query:list")]
        public async Task<IActionResult> Post()
        {
            var actionResult = await this.codeTableService.GetCodeTreeNodes();
            return Ok(actionResult);
        }

        #endregion

        #region 获取字典信息

        /// <summary>
        /// 获取字典信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{id}")]
        [Log(OperationType.QueryEntity)]
        [Permission("code:query:list")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            var codes = await this.redisService.GetAsync<List<SysCodeTableDTO>>(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName);
            actionResult.Object = codes.Where(t => t.Id == id).FirstOrDefault();

            return Ok(actionResult);
        }

        #endregion

        #region 新增字典信息

        /// <summary>
        /// 新增字典信息
        /// </summary>
        /// <param name="codeTable">字典对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("code:add:entity")]
        public async Task<IActionResult> Add([FromBody] SysCodeTable codeTable)
        {
            var actionResult = await this.codeTableService.AddAsync(codeTable);

            //数据一致性维护
            var codes = await this.redisService.GetAsync<List<SysCodeTableDTO>>(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName);
            var codeTableDTO = mapper.Map<SysCodeTableDTO>(codeTable);
            codes.Add(codeTableDTO);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName, codes, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 修改字典信息

        /// <summary>
        /// 修改字典信息
        /// </summary>
        /// <param name="codeTable">字典对象</param>
        /// <returns>IActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("code:edit:entity")]
        public async Task<IActionResult> Put([FromBody] SysCodeTable codeTable)
        {
            var actionResult = await this.codeTableService.UpdateAsync(codeTable);

            //数据一致性维护
            var codes = await this.redisService.GetAsync<List<SysCodeTableDTO>>(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName);
            var code = codes.Where(t => t.Id == codeTable.Id).FirstOrDefault();
            codes.Remove(code);
            var codeTableDTO = mapper.Map<SysCodeTableDTO>(codeTable);
            codes.Add(codeTableDTO);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName, codes, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 删除字典信息

        /// <summary>
        /// 删除字典信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{id}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("code:del:entities")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //字典删除检测
            await this.DeleteCheck(id.ToString());

            //删除字典信息
            var actionResult = await this.codeTableService.DeleteAsync(id);

            //数据一致性维护
            var codes = await this.redisService.GetAsync<List<SysCodeTableDTO>>(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName);
            var code = codes.Where(t => t.Id == id).FirstOrDefault();
            codes.Remove(code);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName, codes, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 批量删除字典信息

        /// <summary>
        /// 批量删除字典信息
        /// </summary>
        /// <param name="ids">数组串</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("code:del:entities")]
        public async Task<IActionResult> DeleteRange(String ids)
        {
            //字典删除检测
            await this.DeleteCheck(ids);

            //删除字典信息
            var array = NGStringUtil.GetGuids(ids);
            var actionResult = await this.codeTableService.DeleteRangeAsync(array);

            //数据一致性维护
            var codes = await this.redisService.GetAsync<List<SysCodeTableDTO>>(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName);
            foreach (var item in array)
            {
                var code = codes.Where(t => t.Id == item).FirstOrDefault();
                codes.Remove(code);
            }
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName, codes, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 获取字典子集

        /// <summary>
        /// 获取字典子集
        /// </summary>
        /// <param name="code">字典编码</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{code}")]
        [Log(OperationType.QueryList)]
        //[Permission("code:query:list")]
        public async Task<IActionResult> GetChildrenByCode(String code)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();

            var codes = await this.redisService.GetAsync<List<SysCodeTableDTO>>(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName);
            var root = codes.Where(t => t.Code.Equals(code)).FirstOrDefault();

            if (root != null)
            {
                actionResult.HttpStatusCode = HttpStatusCode.OK;
                actionResult.Message = new String("OK");
                actionResult.Object = codes.Where(t => t.ParentId.Equals(root.Id)).OrderBy(t => t.SerialNumber).ToList();
            }
            else
            {
                actionResult.HttpStatusCode = HttpStatusCode.OK;
                actionResult.Message = new String("OK");
                actionResult.Object = null;
            }

            return Ok(actionResult);
        }

        #endregion

        #region 字典删除检测

        /// <summary>
        /// 字典删除检测
        /// </summary>
        /// <param name="ids">编号数组</param>
        /// <returns></returns>
        private async Task DeleteCheck(String ids)
        {
            var codes = await this.redisService.GetAsync<List<SysCodeTableDTO>>(NGAdminGlobalContext.SystemCacheConfig.CodeTableCacheName);

            var array = NGStringUtil.GetGuids(ids);
            foreach (var item in array)
            {
                var sysCodes = codes.Where(t => t.ParentId == item).ToList();
                if (sysCodes.Count > 0)
                {
                    throw new NGProjectAdmin.Common.Class.Exceptions.NGAdminCustomException("code contains subitems,can not be deleted");
                }
            }
        }

        #endregion
    }
}
