//using Meilisearch;
//using Microsoft.AspNetCore.Mvc;
////using MongoDB.Driver;
//using NGProjectAdmin.Common.Class.Extensions;
//using NGProjectAdmin.Common.Global;
//using NGProjectAdmin.Common.Utility;
//using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
//using NGProjectAdmin.Entity.BusinessEnum;
//using NGProjectAdmin.Entity.CoreEntity;
////using NGProjectAdmin.Service.BusinessService.MongoDBService;
//using NGProjectAdmin.Service.BusinessService.NestService;
//using NGProjectAdmin.Service.BusinessService.RedisService;
//using NGProjectAdmin.Service.BusinessService.SystemManagement.LogService;
//using NGProjectAdmin.WebApi.AppCode.ActionFilters;
//using NGProjectAdmin.WebApi.AppCode.FrameworkBase;
//using SqlSugar;
//using System;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace NGProjectAdmin.WebApi.Controllers.SystemManagement
//{
//    /// <summary>
//    /// 审计日志管理控制器
//    /// </summary>
//    public class LogManagementController : NGAdminBaseController<SysLog>
//    {
//        #region 属性及构造函数

//        /// <summary>
//        /// 审计日志接口实例
//        /// </summary>
//        private readonly ILogService logService;

      

//        /// <summary>
//        /// Redis接口实例
//        /// </summary>
//        private readonly IRedisService redisService;

//        /// <summary>
//        /// Nest接口实例
//        /// </summary>
//        private readonly INestService NestService;

//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="logService"></param>
//        /// <param name="MongoDBService"></param>
//        /// <param name="NestService"></param>
//        public LogManagementController(ILogService logService, IRedisService MongoDBService,
//                                       INestService NestService) : base(logService)
//        {
//            this.logService = logService;
//            this.MongoDBService = MongoDBService;
//            this.NestService = NestService;
//        }

//        #endregion

//        #region 查询日志列表

//        /// <summary>
//        /// 查询日志列表
//        /// </summary>
//        /// <param name="queryCondition">查询条件</param>
//        /// <returns>ActionResult</returns>
//        [HttpPost]
//        [Log(OperationType.QueryList)]
//        [Permission("log:query:list")]
//        public async Task<IActionResult> Post([FromBody] QueryCondition queryCondition)
//        {
//            if (NGAdminGlobalContext.LogConfig.SupportMongoDB)
//            {
//                #region 从MongoDB获取审计日志

//                <SysLog> mongoCollection = this.MongoDBService.GetBusinessCollection<SysLog>("SysLog");

//                var logs = mongoCollection.AsQueryable().Where(QueryCondition.BuildExpression<SysLog>(queryCondition.QueryItems)).ToList();
//                if (!String.IsNullOrEmpty(queryCondition.Sort))
//                {
//                    logs = logs.Sort<SysLog>(queryCondition.Sort);
//                }

//                var queryList = logs.Skip(queryCondition.PageIndex * queryCondition.PageSize).Take(queryCondition.PageSize).ToList();
//                var actionResult = QueryResult<SysLog>.Success(logs.Count, queryList);

//                return Ok(actionResult);

//                #endregion
//            }
//            else if (NGAdminGlobalContext.LogConfig.SupportElasticsearch)
//            {
//                #region 从Elasticsearch获取审计日志         

//                //**********如果使用keyword从Elasticsearch查询审计日志，请使用以下代码*********************************//

//                ////前端只传递keyword
//                //var keyword = queryCondition.QueryItems.FirstOrDefault().Value.ToString();

//                ////模糊查询
//                //var task1 = await RuYiEsNestContext.Instance.SearchAsync<SysLog>(s => 
//                //s.Query(q => q.Match(m => m.Field(f => f.UserName).Query(keyword)) || 
//                //             q.Match(m => m.Field(f => f.OrgName).Query(keyword))||
//                //             q.Match(m => m.Field(f => f.IP).Query(keyword))||
//                //             q.Match(m => m.Field(f => f.OperationType).Query(keyword))||
//                //             q.Match(m => m.Field(f => f.RequestMethod).Query(keyword))||
//                //             q.Match(m => m.Field(f => f.RequestUrl).Query(keyword))||
//                //             q.Match(m => m.Field(f => f.Params).Query(keyword))||
//                //             q.Match(m => m.Field(f => f.NewValue).Query(keyword))||
//                //             q.Match(m => m.Field(f => f.OldValue).Query(keyword)) ||
//                //             q.Match(m => m.Field(f => f.Result).Query(keyword)) ||
//                //             q.Match(m => m.Field(f => f.Remark).Query(keyword))
//                //       )
//                // );

//                //var logs1 = task1.Documents.ToList();

//                //if (!String.IsNullOrEmpty(queryCondition.Sort))
//                //{
//                //    logs1 = logs1.Sort<SysLog>(queryCondition.Sort);
//                //}
//                //logs1 = logs1.Skip(queryCondition.PageIndex * queryCondition.PageSize).Take(queryCondition.PageSize).ToList();

//                //var actionResult1 = QueryResult<SysLog>.Success((int)task1.Total, logs1);

//                //return Ok(actionResult1);

//                //**********如果使用keyword从Elasticsearch查询审计日志，请使用以上代码查询*********************************//

//                //转化查询条件
//                var query = queryCondition.ToSearchDescriptor<SysLog>();
//                //查询Elasticsearch
//                var searchResponse = await this.NestService.GetElasticClient().SearchAsync<SysLog>(query);
//                var logs = searchResponse.Documents.ToList();
//                var actionResult = QueryResult<SysLog>.Success((int)searchResponse.Total, logs);

//                return Ok(actionResult);

//                #endregion
//            }
//            else if (NGAdminGlobalContext.LogConfig.SupportMeilisearch)
//            {
//                #region 从Meilisearch获取审计日志  

//                //前端只传递keyword
//                var keyword = queryCondition.QueryItems.FirstOrDefault().Value.ToString();
//                var index = NGMeilisearchContext.Instance.GetIndex();

//                SearchResult<SysLog> searchResult = await index.SearchAsync<SysLog>(keyword);

//                var logs = searchResult.Hits.ToList();
//                if (!String.IsNullOrEmpty(queryCondition.Sort))
//                {
//                    logs = logs.Sort<SysLog>(queryCondition.Sort);
//                }
//                logs = logs.Skip(queryCondition.PageIndex * queryCondition.PageSize).Take(queryCondition.PageSize).ToList();

//                var actionResult = QueryResult<SysLog>.Success((int)searchResult.EstimatedTotalHits, logs);
//                return Ok(actionResult);

//                #endregion
//            }
//            else
//            {
//                #region 从关系库获取审计日志

//                var key = "sqls:sql:query_syslog_ms";
//                if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.MySql)
//                {
//                    key = "sqls:sql:query_syslog";
//                }

//                var actionResult = await this.logService.SqlQueryAsync(queryCondition, key);

//                return Ok(actionResult);

//                #endregion
//            }
//        }

//        #endregion

//        #region 获取日志明细

//        /// <summary>
//        /// 获取日志明细
//        /// </summary>
//        /// <param name="logId">日志编号</param>
//        /// <returns>ActionResult</returns>
//        [HttpGet("{logId}")]
//        [Log(OperationType.QueryEntity)]
//        [Permission("log:query:entity")]
//        public async Task<IActionResult> GetById(Guid logId)
//        {
//            var actionResult = await this.logService.GetByIdAsync(logId);
//            return Ok(actionResult);
//        }

//        #endregion

//        #region 下载返回数据

//        /// <summary>
//        /// 下载返回数据
//        /// </summary>
//        /// <param name="txtId">日志编号</param>
//        /// <returns>ActionResult</returns>
//        [HttpGet("{txtId}")]
//        [Log(OperationType.DownloadFile)]
//        public async Task<IActionResult> DownloadMonitoringLog(String txtId)
//        {
//            return await Task.Run(() =>
//            {
//                //存储路径
//                var path = Path.Join(NGAdminGlobalContext.DirectoryConfig.GetMonitoringLogsPath(), "/");
//                //文件名称
//                var fileName = txtId + ".txt";
//                //文件路径
//                var filePath = Path.Join(path, fileName);
//                //文件读写流
//                var stream = new FileStream(filePath, FileMode.Open);
//                //设置流的起始位置
//                stream.Position = 0;

//                return File(stream, "application/octet-stream", fileName);
//            });
//        }

//        #endregion
//    }
//}
