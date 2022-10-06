
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NGProjectAdmin.Common.Utility;
using NGProjectAdmin.Entity.BusinessDTO.SystemManagement;
using NGProjectAdmin.Entity.BusinessEntity.System;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.BusinessEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Service.BusinessService.MQService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.AddresseeService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.AnnouncementService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.AttachmentService;
using NGProjectAdmin.Service.BusinessService.SystemManagement.UserService;
using NGProjectAdmin.WebApi.AppCode.ActionFilters;
using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
{
    /// <summary>
    /// 通知公告管理控制器
    /// </summary>
    public class AnnouncementManagementController : NGAdminBaseController<SysAnnouncement>
    {
        #region 属性及构造函数

        /// <summary>
        /// 通知公告业务接口实例
        /// </summary>
        private readonly IAnnouncementService announcementService;

        /// <summary>
        /// AutoMapper实例
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 收件人服务层接口实例
        /// </summary>
        private readonly IAddresseeService addresseeService;

        /// <summary>
        /// 系统用户服务层接口实例
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// 系统附件服务层接口实例
        /// </summary>
        private readonly IAttachmentService attachmentService;

        /// <summary>
        /// MQ接口实例
        /// </summary>
        private readonly IMQService mqService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="announcementService"></param>
        /// <param name="mapper"></param>
        /// <param name="addresseeService"></param>
        /// <param name="userService"></param>
        /// <param name="attachmentService"></param>
        /// <param name="mqService"></param>
        public AnnouncementManagementController(IAnnouncementService announcementService,
                                                IMapper mapper,
                                                IAddresseeService addresseeService,
                                                IUserService userService,
                                                IAttachmentService attachmentService,
                                                IMQService mqService) : base(announcementService)
        {
            this.announcementService = announcementService;
            this.mapper = mapper;
            this.addresseeService = addresseeService;
            this.userService = userService;
            this.attachmentService = attachmentService;
            this.mqService = mqService;
        }

        #endregion

        #region 查询通知公告列表

        /// <summary>
        /// 查询通知公告列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("announcement:query:list")]
        public async Task<IActionResult> Post(QueryCondition queryCondition)
        {
            var actionResult = await this.announcementService.SqlQueryAsync(queryCondition, "sqls:sql:query_sysannouncement");
            return Ok(actionResult);
        }

        #endregion

        #region 按编号获取通知公告

        /// <summary>
        /// 按编号获取通知公告
        /// </summary>
        /// <param name="id">通知公告编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{id}")]
        [Log(OperationType.QueryEntity)]
        [Permission("announcement:query:list,sys:announcement:query:list,notification:query:list")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var actionResult = await this.announcementService.GetByIdAsync(id);
            return Ok(actionResult);
        }

        #endregion

        #region 添加通知公告

        /// <summary>
        /// 添加通知公告
        /// </summary>
        /// <param name="announcementDTO">通知公告</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.AddEntity)]
        [Permission("announcement:add:entity")]
        public async Task<IActionResult> Add([FromBody] SysAnnouncementDTO announcementDTO)
        {
            //数据转化
            var announcement = mapper.Map<SysAnnouncement>(announcementDTO);
            //保存数据
            var actionResult = await this.announcementService.AddAsync(announcement);
            //保存通知收件人并发送邮件
            await this.SengMailAndMessageAsync(announcementDTO);

            return Ok(actionResult);
        }

        #endregion

        #region 编辑通知公告

        /// <summary>
        /// 编辑通知公告
        /// </summary>
        /// <param name="announcementDTO">通知公告</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        [Log(OperationType.EditEntity)]
        [Permission("announcement:edit:entity")]
        public async Task<IActionResult> Edit([FromBody] SysAnnouncementDTO announcementDTO)
        {
            var announcement = mapper.Map<SysAnnouncement>(announcementDTO);
            var actionResult = await this.announcementService.UpdateAsync(announcement);

            #region 删除收件人

            var queryCondition = new QueryCondition();
            queryCondition.QueryItems = new List<QueryItem>();
            queryCondition.QueryItems.Add(new QueryItem()
            {
                Field = "BusinessId",
                QueryMethod = Entity.CoreEnum.QueryMethod.Equal,
                DataType = Entity.CoreEnum.DataType.Guid,
                Value = announcement.Id
            });
            var list = this.addresseeService.GetList(queryCondition).List;
            foreach (var addressee in list)
            {
                actionResult = await this.addresseeService.DeleteAsync(addressee.Id);
            }

            #endregion

            //保存通知收件人并发送邮件
            await this.SengMailAndMessageAsync(announcementDTO);

            return Ok(actionResult);
        }

        #endregion

        #region 批量删除通知公告

        /// <summary>
        /// 批量删除通知公告
        /// </summary>
        /// <param name="ids">数组串</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{ids}")]
        [Log(OperationType.DeleteEntity)]
        [Permission("announcement:del:entities")]
        public async Task<IActionResult> Delete(String ids)
        {
            var array = NGStringUtil.GetGuids(ids);

            //批量删除通知公告
            var actionResult = await this.announcementService.DeleteRangeAsync(array);

            foreach (var item in array)
            {
                var queryCondition = new QueryCondition();
                queryCondition.QueryItems = new List<QueryItem>();
                queryCondition.QueryItems.Add(new QueryItem()
                {
                    Field = "BusinessId",
                    QueryMethod = Entity.CoreEnum.QueryMethod.Equal,
                    DataType = Entity.CoreEnum.DataType.Guid,
                    Value = item
                });
                var list = this.addresseeService.GetList(queryCondition).List;
                foreach (var addressee in list)
                {
                    //删除收件人
                    actionResult = await this.addresseeService.DeleteAsync(addressee.Id);
                }
            }

            return Ok(actionResult);
        }

        #endregion

        #region 获取通知公告收件人

        /// <summary>
        /// 获取通知公告收件人
        /// </summary>
        /// <param name="id">通知公告编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{id}")]
        [Log(OperationType.QueryList)]
        [Permission("announcement:edit:entity")]
        public async Task<IActionResult> GetAnnouncementAddressee(Guid id)
        {
            var queryCondition = new QueryCondition();
            queryCondition.QueryItems = new List<QueryItem>();
            queryCondition.QueryItems.Add(new QueryItem()
            {
                Field = "BusinessId",
                QueryMethod = Entity.CoreEnum.QueryMethod.Equal,
                DataType = Entity.CoreEnum.DataType.Guid,
                Value = id
            });

            var actionResult = await this.addresseeService.GetListAsync(queryCondition);
            return Ok(actionResult);
        }

        #endregion

        #region 查询公告列表

        /// <summary>
        /// 查询公告列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("sys:announcement:query:list")]
        public async Task<IActionResult> GetAnnouncements(QueryCondition queryCondition)
        {
            var actionResult = await this.announcementService.SqlQueryAsync(queryCondition, "sqls:sql:query_sys_announcement");
            return Ok(actionResult);
        }

        #endregion

        #region 查询通知列表

        /// <summary>
        /// 查询通知列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Log(OperationType.QueryList)]
        [Permission("notification:query:list")]
        public async Task<IActionResult> GetNotifications(QueryCondition queryCondition)
        {
            var actionResult = await this.announcementService.QueryNotifications(queryCondition, "sqls:sql:query_sys_notification");
            return Ok(actionResult);
        }

        #endregion

        #region 更改通知收件人阅读状态

        /// <summary>
        /// 更改通知收件人阅读状态
        /// </summary>
        /// <param name="notificationId">通知编号</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{notificationId}")]
        [Log(OperationType.EditEntity)]
        [Permission("notification:query:list")]
        public async Task<IActionResult> UpdateNotificationStatus(Guid notificationId)
        {
            var actionResult = await this.addresseeService.UpdateNotificationStatus(notificationId);
            return Ok(actionResult);
        }

        #endregion

        #region 发送邮件与及时消息

        private async Task SengMailAndMessageAsync(SysAnnouncementDTO announcementDTO)
        {
            if (announcementDTO.Type == 0 && announcementDTO.Status == 0)
            {
                #region 发送及时消息

                var msg = new SystemMessage();
                msg.Message = "Announcement";
                msg.MessageType = MessageType.Announcement;
                msg.Object = new { Title = announcementDTO.Title };

                this.mqService.SendTopic(JsonConvert.SerializeObject(msg));

                #endregion
            }
            else if (announcementDTO.Type == 1 && !String.IsNullOrEmpty(announcementDTO.Addressee))
            {
                #region 发送邮件与及时消息

                var array = NGStringUtil.GetGuids(announcementDTO.Addressee);

                var addressee = new List<SysAddressee>();
                foreach (var item in array)
                {
                    var addr = new SysAddressee();
                    addr.BusinessId = announcementDTO.Id;
                    addr.UserId = item;
                    addr.Status = 0;
                    addressee.Add(addr);
                }
                await this.addresseeService.AddListAsync(addressee);

                //发送邮件
                if (announcementDTO.SendMail && announcementDTO.Status == 0 && addressee.Count > 0)
                {
                    foreach (var item in array)
                    {
                        var user = this.userService.GetById(item).Object as SysUser;
                        if (!String.IsNullOrEmpty(user.Email))
                        {
                            //获取附件列表
                            var attachments = this.attachmentService.GetList().Object as List<SysAttachment>;
                            attachments = attachments.FindAll(t => t.BusinessId == announcementDTO.Id && t.IsDel == 0);

                            //加载附件信息
                            List<FileInfo> files = new List<FileInfo>();
                            foreach (var att in attachments)
                            {
                                files.Add(new FileInfo(att.FilePath));
                            }

                            //发送邮件
                         //   NGMailUtil.SendMail(announcementDTO.Title, announcementDTO.Content, user.Email, files, true);

                            #region 发送及时消息

                            var msg = new SystemMessage();
                            msg.Message = "Notification";
                            msg.MessageType = MessageType.Notification;
                            msg.Object = new { Id = user.Id, Title = announcementDTO.Title };

                            this.mqService.SendTopic(JsonConvert.SerializeObject(msg));

                            #endregion
                        }
                    }
                }

                #endregion
            }
        }

        #endregion
    }
}
