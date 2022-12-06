using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.CoreEntity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NGProjectAdmin.Service.Base
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IBaseService<T> where T :BaseEntity
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        QueryResult<T> GetPage(QueryCondition queryCondition);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        QueryResult<T> GetList(QueryCondition queryCondition);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>实体</returns>
        ActionResult GetById(int id);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <returns>查询结果</returns>
        QueryResult<T> SqlQuery(QueryCondition queryCondition, String sqlKey);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        QueryResult<T> SqlQuery(String strSQL, QueryCondition queryCondition);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <returns>查询结果</returns>
        QueryResult<TDTO> SqlQuery<TDTO>(QueryCondition queryCondition, String sqlKey) where TDTO : class, new();

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="strSQL">查询语句</param>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        QueryResult<TDTO> SqlQuery<TDTO>(String strSQL, QueryCondition queryCondition) where TDTO : class, new();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>执行结果</returns>
        ActionResult GetList();

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <returns>查询结果</returns>
        QueryResult<TDTO> GetList<TDTO>() where TDTO : class, new();

        /// <summary>
        /// 按表达式查询
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>执行结果</returns>
        ActionResult QueryByExpression(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="create">是否赋值</param>
        /// <returns>执行结果</returns>
        ActionResult Add(T obj, bool create = true);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="create">是否赋值</param>
        /// <returns>执行结果</returns>
        ActionResult AddList(List<T> list, bool create = true);

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>执行结果</returns>
        ActionResult Update(T obj);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>执行结果</returns>
        ActionResult UpdateList(List<T> list);

        /// <summary>
        /// 逻辑删除对象
        /// </summary>
        /// <param name="id">对象编号</param>
        /// <returns>执行结果</returns>
        ActionResult Delete(int id);

        /// <summary>
        /// 批量逻辑删除对象
        /// </summary>
        /// <param name="ids">对象编号数组</param>
        /// <returns>执行结果</returns>
        ActionResult DeleteRange(int[] ids);

        /// <summary>
        /// 物理删除对象
        /// </summary>
        /// <param name="id">对象编号</param>
        /// <returns>执行结果</returns>
        ActionResult Remove(int id);

        /// <summary>
        /// 批量物理删除对象
        /// </summary>
        /// <param name="ids">对象编号数组</param>
        /// <returns>执行结果</returns>
        ActionResult RemoveRange(int[] ids);

        /// <summary>
        /// 大数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        ActionResult BulkCopy(List<T> list);

        /// <summary>
        /// 大数据分页写入
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        ActionResult PageBulkCopy(int pageSize, List<T> list);

        /// <summary>
        /// 大数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        ActionResult BulkUpdate(List<T> list);

        /// <summary>
        /// 海量数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        ActionResult HugeDataBulkCopy(List<T> list);

        /// <summary>
        /// 海量数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        ActionResult HugeDataBulkUpdate(List<T> list);

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        DbResult<bool> UseTransaction(Action action, Action<Exception> errorCallBack = null);

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <typeparam name="TEntity">数据类型</typeparam>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        DbResult<TEntity> UseTransaction<TEntity>(Func<TEntity> action, Action<Exception> errorCallBack = null) where TEntity : NGAdminBaseEntity;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        Task<QueryResult<T>> GetPageAsync(QueryCondition queryCondition);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        Task<QueryResult<T>> GetListAsync(QueryCondition queryCondition);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>实体</returns>
        Task<ActionResult> GetByIdAsync(string id);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <returns>查询结果</returns>
        Task<QueryResult<T>> SqlQueryAsync(QueryCondition queryCondition, String sqlKey);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        Task<QueryResult<T>> SqlQueryAsync(String strSQL, QueryCondition queryCondition);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <returns>查询结果</returns>
        Task<QueryResult<TDTO>> SqlQueryAsync<TDTO>(QueryCondition queryCondition, String sqlKey) where TDTO : class, new();

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="strSQL">sql语句</param>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        Task<QueryResult<TDTO>> SqlQueryAsync<TDTO>(String strSQL, QueryCondition queryCondition) where TDTO : class, new();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        Task<ActionResult> GetListAsync();

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <returns>查询结果</returns>
        Task<QueryResult<TDTO>> GetListAsync<TDTO>() where TDTO : class, new();

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="create">是否赋值</param>
        /// <returns>执行结果</returns>
        Task<ActionResult> AddAsync(T obj, bool create = true);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="create">是否赋值</param>
        /// <returns>执行结果</returns>
        Task<ActionResult> AddListAsync(List<T> list, bool create = true);

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>执行结果</returns>
        Task<ActionResult> UpdateAsync(T obj);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>执行结果</returns>
        Task<ActionResult> UpdateListAsync(List<T> list);

        /// <summary>
        /// 逻辑删除对象
        /// </summary>
        /// <param name="id">对象编号</param>
        /// <returns>执行结果</returns>
        Task<ActionResult> DeleteAsync(string id);

        /// <summary>
        /// 批量逻辑删除对象
        /// </summary>
        /// <param name="ids">对象编号数组</param>
        /// <returns>执行结果</returns>
        Task<ActionResult> DeleteRangeAsync(int[] ids);

        /// <summary>
        /// 物理删除对象
        /// </summary>
        /// <param name="id">对象编号</param>
        /// <returns>执行结果</returns>
        Task<ActionResult> RemoveAsync(int id);

        /// <summary>
        /// 批量物理删除对象
        /// </summary>
        /// <param name="ids">对象编号数组</param>
        /// <returns>执行结果</returns>
        Task<ActionResult> RemoveRangeAsync(int[] ids);

        /// <summary>
        /// 大数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> BulkCopyAsync(List<T> list);

        /// <summary>
        /// 大数据分页写入
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> PageBulkCopyAsync(int pageSize, List<T> list);

        /// <summary>
        /// 大数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> BulkUpdateAsync(List<T> list);

        /// <summary>
        /// 海量数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> HugeDataBulkCopyAsync(List<T> list);

        /// <summary>
        /// 海量数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>ActionResult</returns>
        Task<ActionResult> HugeDataBulkUpdateAsync(List<T> list);

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        Task<DbResult<bool>> UseTransactionAsync(Func<Task> action, Action<Exception> errorCallBack = null);

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <typeparam name="TEntity">数据类型</typeparam>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        Task<DbResult<TEntity>> UseTransactionAsync<TEntity>(Func<Task<TEntity>> action, Action<Exception> errorCallBack = null) where TEntity :NGAdminBaseEntity;
    }
}
