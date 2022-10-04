
using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.Base
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public class NGAdminBaseService<T> : INGAdminBaseService<T> where T :NGAdminBaseEntity
    {
        #region 属性及构造函数

        /// <summary>
        /// 基类仓储实例
        /// </summary>
        private readonly INGAdminBaseRepository<T> NGAdminBaseRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="NGAdminBaseRepository"></param>
        public NGAdminBaseService(INGAdminBaseRepository<T> NGAdminBaseRepository)
        {
            this.NGAdminBaseRepository = NGAdminBaseRepository;
        }

        #endregion

        #region 同步方法

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public QueryResult<T> GetPage(QueryCondition queryCondition)
        {
            var totalCount = 0;

            var queryResult = new QueryResult<T>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = this.NGAdminBaseRepository.GetPage(queryCondition, ref totalCount);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public QueryResult<T> GetList(QueryCondition queryCondition)
        {
            var queryResult = new QueryResult<T>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = this.NGAdminBaseRepository.GetList(queryCondition);
            queryResult.TotalCount = queryResult.List.Count;

            return queryResult;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>实体</returns>
        public ActionResult GetById(Guid id)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.GetById(id);

            return actionResult;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <returns>查询结果</returns>
        public QueryResult<T> SqlQuery(QueryCondition queryCondition, string sqlKey)
        {
            var totalCount = 0;

            var queryResult = new QueryResult<T>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = this.NGAdminBaseRepository.SqlQuery(queryCondition, sqlKey, ref totalCount);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public QueryResult<T> SqlQuery(String strSQL, QueryCondition queryCondition)
        {
            var totalCount = 0;

            var queryResult = new QueryResult<T>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = this.NGAdminBaseRepository.SqlQuery(queryCondition, ref totalCount, strSQL);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <returns>查询结果</returns>
        public QueryResult<TDTO> SqlQuery<TDTO>(QueryCondition queryCondition, String sqlKey) where TDTO : class, new()
        {
            var totalCount = 0;

            var queryResult = new QueryResult<TDTO>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = this.NGAdminBaseRepository.SqlQuery<TDTO>(queryCondition, sqlKey, ref totalCount);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="strSQL">查询语句</param>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public QueryResult<TDTO> SqlQuery<TDTO>(String strSQL, QueryCondition queryCondition) where TDTO : class, new()
        {
            var totalCount = 0;

            var queryResult = new QueryResult<TDTO>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = this.NGAdminBaseRepository.SqlQuery<TDTO>(queryCondition, ref totalCount, strSQL);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>执行结果</returns>
        public ActionResult GetList()
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.GetList();

            return actionResult;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <returns>查询结果</returns>
        public QueryResult<TDTO> GetList<TDTO>() where TDTO : class, new()
        {
            var queryResult = new QueryResult<TDTO>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            var list = this.NGAdminBaseRepository.GetList<TDTO>();
            queryResult.List = list;
            queryResult.TotalCount = list.Count;

            return queryResult;
        }

        /// <summary>
        /// 按表达式查询
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>执行结果</returns>
        public ActionResult QueryByExpression(Expression<Func<T, bool>> expression)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.QueryByExpression(expression);

            return actionResult;
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="create">是否赋值</param>
        /// <returns>执行结果</returns>
        public ActionResult Add(T obj, bool create = true)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.AddEntity(obj, create);

            return actionResult;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="create">是否赋值</param>
        /// <returns>执行结果</returns>
        public ActionResult AddList(List<T> list, bool create = true)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.AddList(list, create);

            return actionResult;
        }

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>执行结果</returns>
        public ActionResult Update(T obj)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.UpdateEntity(obj);

            return actionResult;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>执行结果</returns>
        public ActionResult UpdateList(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.UpdateList(list);

            return actionResult;
        }

        /// <summary>
        /// 逻辑删除对象
        /// </summary>
        /// <param name="id">对象编号</param>
        /// <returns>执行结果</returns>
        public ActionResult Delete(Guid id)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.DeleteEntity(id);

            return actionResult;
        }

        /// <summary>
        /// 批量逻辑删除对象
        /// </summary>
        /// <param name="ids">对象编号数组</param>
        /// <returns>执行结果</returns>
        public ActionResult DeleteRange(Guid[] ids)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.DeleteRange(ids);

            return actionResult;
        }

        /// <summary>
        /// 物理删除对象
        /// </summary>
        /// <param name="id">对象编号</param>
        /// <returns>执行结果</returns>
        public ActionResult Remove(Guid id)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.RemoveEntity(id);

            return actionResult;
        }

        /// <summary>
        /// 批量物理删除对象
        /// </summary>
        /// <param name="ids">对象编号数组</param>
        /// <returns>执行结果</returns>
        public ActionResult RemoveRange(Guid[] ids)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.RemoveRange(ids);

            return actionResult;
        }

        /// <summary>
        /// 大数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public ActionResult BulkCopy(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.BulkCopy(list);

            return actionResult;
        }

        /// <summary>
        /// 大数据分页写入
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public ActionResult PageBulkCopy(int pageSize, List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.PageBulkCopy(pageSize, list);

            return actionResult;
        }

        /// <summary>
        /// 大数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public ActionResult BulkUpdate(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.BulkUpdate(list);

            return actionResult;
        }

        /// <summary>
        /// 海量数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public ActionResult HugeDataBulkCopy(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.HugeDataBulkCopy(list);

            return actionResult;
        }

        /// <summary>
        /// 海量数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public ActionResult HugeDataBulkUpdate(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = this.NGAdminBaseRepository.HugeDataBulkUpdate(list);

            return actionResult;
        }

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        public DbResult<bool> UseTransaction(Action action, Action<Exception> errorCallBack = null)
        {
            return this.NGAdminBaseRepository.UseTransaction(action, errorCallBack);
        }

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <typeparam name="TEntity">数据类型</typeparam>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        public DbResult<TEntity> UseTransaction<TEntity>(Func<TEntity> action, Action<Exception> errorCallBack = null) where TEntity : NGAdminBaseEntity
        {
            return this.NGAdminBaseRepository.UseTransaction(action, errorCallBack);
        }

        #endregion

        #region 异步方法

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public async Task<QueryResult<T>> GetPageAsync(QueryCondition queryCondition)
        {
            RefAsync<int> totalCount = 0;

            var queryResult = new QueryResult<T>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = await this.NGAdminBaseRepository.GetPageAsync(queryCondition, totalCount);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public async Task<QueryResult<T>> GetListAsync(QueryCondition queryCondition)
        {
            var queryResult = new QueryResult<T>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = await this.NGAdminBaseRepository.GetListAsync(queryCondition);
            queryResult.TotalCount = queryResult.List.Count;

            return queryResult;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>实体</returns>
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.GetByIdAsync(id);

            return actionResult;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <returns>查询结果</returns>
        public async Task<QueryResult<T>> SqlQueryAsync(QueryCondition queryCondition, String sqlKey)
        {
            RefAsync<int> totalCount = 0;

            var queryResult = new QueryResult<T>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = await this.NGAdminBaseRepository.SqlQueryAsync(queryCondition, sqlKey, totalCount);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public async Task<QueryResult<T>> SqlQueryAsync(String strSQL, QueryCondition queryCondition)
        {
            RefAsync<int> totalCount = 0;

            var queryResult = new QueryResult<T>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = await this.NGAdminBaseRepository.SqlQueryAsync(queryCondition, totalCount, strSQL);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }
        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <returns>查询结果</returns>
        public async Task<QueryResult<TDTO>> SqlQueryAsync<TDTO>(QueryCondition queryCondition, String sqlKey) where TDTO : class, new()
        {
            RefAsync<int> totalCount = 0;

            var queryResult = new QueryResult<TDTO>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = await this.NGAdminBaseRepository.SqlQueryAsync<TDTO>(queryCondition, sqlKey, totalCount);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="strSQL">sql语句</param>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public async Task<QueryResult<TDTO>> SqlQueryAsync<TDTO>(String strSQL, QueryCondition queryCondition) where TDTO : class, new()
        {
            RefAsync<int> totalCount = 0;

            var queryResult = new QueryResult<TDTO>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            queryResult.List = await this.NGAdminBaseRepository.SqlQueryAsync<TDTO>(queryCondition, totalCount, strSQL);
            queryResult.TotalCount = totalCount;

            return queryResult;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        public async Task<ActionResult> GetListAsync()
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.GetListAsync();

            return actionResult;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <returns>查询结果</returns>
        public async Task<QueryResult<TDTO>> GetListAsync<TDTO>() where TDTO : class, new()
        {
            var queryResult = new QueryResult<TDTO>();

            queryResult.HttpStatusCode = HttpStatusCode.OK;
            queryResult.Message = new String("OK");
            var list = await this.NGAdminBaseRepository.GetListAsync<TDTO>();
            queryResult.List = list;
            queryResult.TotalCount = list.Count;

            return queryResult;
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="create">是否赋值</param>
        /// <returns>执行结果</returns>
        public async Task<ActionResult> AddAsync(T obj, bool create = true)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.AddEntityAsync(obj, create);

            return actionResult;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="create">是否赋值</param>
        /// <returns>执行结果</returns>
        public async Task<ActionResult> AddListAsync(List<T> list, bool create = true)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.AddListAsync(list, create);

            return actionResult;
        }

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>执行结果</returns>
        public async Task<ActionResult> UpdateAsync(T obj)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.UpdateEntityAsync(obj);

            return actionResult;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>执行结果</returns>
        public async Task<ActionResult> UpdateListAsync(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.UpdateListAsync(list);

            return actionResult;
        }

        /// <summary>
        /// 逻辑删除对象
        /// </summary>
        /// <param name="id">对象编号</param>
        /// <returns>执行结果</returns>
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.DeleteEntityAsync(id);

            return actionResult;
        }

        /// <summary>
        /// 批量逻辑删除对象
        /// </summary>
        /// <param name="ids">对象编号数组</param>
        /// <returns>执行结果</returns>
        public async Task<ActionResult> DeleteRangeAsync(Guid[] ids)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.DeleteRangeAsync(ids);

            return actionResult;
        }

        /// <summary>
        /// 物理删除对象
        /// </summary>
        /// <param name="id">对象编号</param>
        /// <returns>执行结果</returns>
        public async Task<ActionResult> RemoveAsync(Guid id)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.RemoveEntityAsync(id);

            return actionResult;
        }

        /// <summary>
        /// 批量物理删除对象
        /// </summary>
        /// <param name="ids">对象编号数组</param>
        /// <returns>执行结果</returns>
        public async Task<ActionResult> RemoveRangeAsync(Guid[] ids)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.RemoveRangeAsync(ids);

            return actionResult;
        }

        /// <summary>
        /// 大数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> BulkCopyAsync(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.BulkCopyAsync(list);

            return actionResult;
        }

        /// <summary>
        /// 大数据分页写入
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> PageBulkCopyAsync(int pageSize, List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.PageBulkCopyAsync(pageSize, list);

            return actionResult;
        }

        /// <summary>
        /// 大数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> BulkUpdateAsync(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.BulkUpdateAsync(list);

            return actionResult;
        }

        /// <summary>
        /// 海量数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> HugeDataBulkCopyAsync(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.HugeDataBulkCopyAsync(list);

            return actionResult;
        }

        /// <summary>
        /// 海量数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        public async Task<ActionResult> HugeDataBulkUpdateAsync(List<T> list)
        {
            var actionResult = new ActionResult();

            actionResult.HttpStatusCode = HttpStatusCode.OK;
            actionResult.Message = new String("OK");
            actionResult.Object = await this.NGAdminBaseRepository.HugeDataBulkUpdateAsync(list);

            return actionResult;
        }


        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        public async Task<DbResult<bool>> UseTransactionAsync(Func<Task> action, Action<Exception> errorCallBack = null)
        {
            return await this.NGAdminBaseRepository.UseTransactionAsync(action, errorCallBack);
        }

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <typeparam name="TEntity">数据类型</typeparam>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        public async Task<DbResult<TEntity>> UseTransactionAsync<TEntity>(Func<Task<TEntity>> action, Action<Exception> errorCallBack = null) where TEntity : NGAdminBaseEntity
        {
            return await this.NGAdminBaseRepository.UseTransactionAsync(action, errorCallBack);
        }

        #endregion
    }
}

