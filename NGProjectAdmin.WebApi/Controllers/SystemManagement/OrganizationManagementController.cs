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
using NGProjectAdmin.Service.BusinessService.SystemManagement.OrganizationService;
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
    /// 机构管理控制器
    /// </summary>
    public class OrganizationManagementController : NGAdminBaseController<SysOrganization>
    {
        #region 属性及构造函数

        /// <summary>
        /// 机构接口实例
        /// </summary>
        private readonly IOrganizationService organizationService;

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
        /// <param name="organizationService"></param>
        /// <param name="redisService"></param>
        /// <param name="mapper"></param>
        public OrganizationManagementController(IOrganizationService organizationService,
                                                IRedisService redisService,
                                                IMapper mapper) : base(organizationService)
        {
            this.organizationService = organizationService;
            this.redisService = redisService;
            this.mapper = mapper;
        }

        #endregion

        #region 查询机构列表

        /// <summary>
        /// 查询机构列表
        /// </summary>
        /// <returns>QueryResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("org:query:list")]
        public async Task<IActionResult> Post()
        {
            var actionResult = await this.organizationService.GetOrgTreeNodes();
            return Ok(actionResult);
        }

        #endregion

        #region 查询机构信息

        /// <summary>
        /// 查询机构信息
        /// </summary>
        /// <param name="orgId">机构编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{orgId}")]
        [Log(OperationType.QueryEntity)]
        [Permission("org:query:list")]
        public async Task<IActionResult> GetById(Guid orgId)
        {
            var actionResult = new Entity.CoreEntity.ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            var orgs = await this.redisService.GetAsync<List<SysOrganizationDTO>>(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName);
            actionResult.Object = orgs.Where(t => t.Id == orgId).FirstOrDefault();

            return Ok(actionResult);
        }

        #endregion

        #region 新增机构信息

        /// <summary>
        /// 新增机构信息
        /// </summary>
        /// <param name="org">机构对象</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("org:add:entity")]
        public async Task<IActionResult> Add([FromBody] SysOrganization org)
        {
            var actionResult = await this.organizationService.AddAsync(org);

            //数据一致性维护
            var orgs = await this.redisService.GetAsync<List<SysOrganizationDTO>>(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName);
            var orgDTO = mapper.Map<SysOrganizationDTO>(org);
            orgs.Add(orgDTO);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName, orgs, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 编辑机构信息

        /// <summary>
        /// 编辑机构信息
        /// </summary>
        /// <param name="org">机构对象</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("org:edit:entity")]
        public async Task<IActionResult> Put([FromBody] SysOrganization org)
        {
            var actionResult = await this.organizationService.UpdateAsync(org);

            #region 数据一致性维护

            //获取机构缓存
            var orgs = await this.redisService.GetAsync<List<SysOrganizationDTO>>(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName);
            //删除旧数据
            var old = orgs.Where(t => t.Id == org.Id).FirstOrDefault();
            orgs.Remove(old);
            //转化为DTO
            var orgDTO = mapper.Map<SysOrganizationDTO>(org);
            if (orgDTO.Leader != null)
            {
                var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
                var user = users.Where(t => t.Id == orgDTO.Leader).FirstOrDefault();
                if (user != null)
                {
                    //机构管理人姓名赋值
                    orgDTO.LeaderName = user.DisplayName;
                }
            }
            //添加新数据
            orgs.Add(orgDTO);
            //更新机构缓存
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName, orgs, -1);

            //机构名称变更时修改用户缓存的机构名称
            if (old.OrgName != orgDTO.OrgName)
            {
                var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
                var orgUsers = users.Where(t => t.OrgId == orgDTO.Id).ToList();

                foreach (var item in orgUsers)
                {
                    //删除旧用户
                    var oldUser = item;
                    users.Remove(oldUser);

                    //添加新用户
                    item.OrgName = orgDTO.OrgName;
                    users.Add(item);
                }

                //更新用户缓存
                await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.UserCacheName, users, -1);
            }

            #endregion

            return Ok(actionResult);
        }

        #endregion

        #region 删除机构信息

        /// <summary>
        /// 删除机构信息
        /// </summary>
        /// <param name="orgId">对象编号</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{orgId}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("org:del:entities")]
        public async Task<IActionResult> Delete(Guid orgId)
        {
            await this.DeleteCheck(orgId.ToString());

            var actionResult = await this.organizationService.DeleteAsync(orgId);

            //数据一致性维护
            var orgs = await this.redisService.GetAsync<List<SysOrganizationDTO>>(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName);
            var org = orgs.Where(t => t.Id == orgId).FirstOrDefault();
            orgs.Remove(org);
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName, orgs, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 批量删除机构

        /// <summary>
        /// 批量删除机构
        /// </summary>
        /// <param name="ids">编号数组</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("org:del:entities")]
        public async Task<IActionResult> DeleteRange(String ids)
        {
            await this.DeleteCheck(ids);

            var array = NGStringUtil.GetGuids(ids);
            var actionResult = await this.organizationService.DeleteRangeAsync(array);

            //数据一致性维护
            var orgs = await this.redisService.GetAsync<List<SysOrganizationDTO>>(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName);
            foreach (var item in array)
            {
                var org = orgs.Where(t => t.Id == item).FirstOrDefault();
                orgs.Remove(org);
            }
            await this.redisService.SetAsync(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName, orgs, -1);

            return Ok(actionResult);
        }

        #endregion

        #region 获取机构用户树

        /// <summary>
        /// 获取机构用户树
        /// </summary>
        /// <returns>QueryResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        //[Permission("role:query:list,user:query:list")]
        public async Task<IActionResult> GetOrgUserTree()
        {
            var actionResult = await this.organizationService.GetOrgUserTree();
            return Ok(actionResult);
        }

        #endregion

        #region 机构删除检测

        /// <summary>
        /// 机构删除检测
        /// </summary>
        /// <param name="ids">编号数组</param>
        /// <returns></returns>
        private async Task DeleteCheck(String ids)
        {
            var users = await this.redisService.GetAsync<List<SysUserDTO>>(NGAdminGlobalContext.SystemCacheConfig.UserCacheName);
            var orgs = await this.redisService.GetAsync<List<SysOrganizationDTO>>(NGAdminGlobalContext.SystemCacheConfig.OrgCacheName);

            var array = NGStringUtil.GetGuids(ids);
            foreach (var item in array)
            {
                var userCount = users.Where(t => t.OrgId == item).Count();
                var orgCount = orgs.Where(t => t.ParentId == item).Count();

                if (userCount > 0 || orgCount > 0)
                {
                    throw new NGAdminCustomException("org contains users or sub orgs,can not be deleted");
                }
            }
        }

        #endregion
    }
}
