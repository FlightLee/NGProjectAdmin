using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.CoreEntity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NGProjectAdmin.Repository.Base
{
    /// <summary>
    /// 数据库访问接口
    /// </summary>
    public interface IBaseRepository<T> where T : BaseEntity
    {
      
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        List<T> GetPage(QueryCondition queryCondition, ref int totalCount);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        List<T> GetList(QueryCondition queryCondition);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>实体</returns>
        T GetById(int id);


        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        List<T> SqlQuery(QueryCondition queryCondition, String sqlKey, ref int totalCount);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录条数</param>
        /// <param name="strSQL">查询语句</param>
        /// <returns>查询结果</returns>
        List<T> SqlQuery(QueryCondition queryCondition, ref int totalCount, String strSQL);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <param name="totalCount">记录总数</param>
        /// <returns>查询结果</returns>
        List<TDTO> SqlQuery<TDTO>(QueryCondition queryCondition, String sqlKey, ref int totalCount) where TDTO : class, new();

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录总数</param>
        /// <param name="strSQL">sql语句</param>
        /// <returns>查询结果</returns>
        List<TDTO> SqlQuery<TDTO>(QueryCondition queryCondition, ref int totalCount, String strSQL) where TDTO : class, new();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        List<T> GetList();

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <returns>查询结果</returns>
        List<TDTO> GetList<TDTO>() where TDTO : class, new();

        /// <summary>
        /// 按表达式查询
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>列表</returns>
        List<T> QueryByExpression(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="create">是否赋值</param>
        /// <returns>对象</returns>
        T AddEntity(T obj, bool create = true);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="create">是否赋值</param>
        /// <returns>集合</returns>
        List<T> AddList(List<T> list, bool create = true);

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>对象</returns>
        T UpdateEntity(T obj);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>集合</returns>
        List<T> UpdateList(List<T> list);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>标志</returns>
        bool DeleteEntity(int id);

        /// <summary>
        /// 逻辑批量删除
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>标志位</returns>
        bool DeleteRange(int[] ids);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>标志</returns>
        bool RemoveEntity(int id);

        /// <summary>
        /// 物理批量删除
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>标志</returns>
        bool RemoveRange(int[] ids);

        /// <summary>
        /// 大数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        int BulkCopy(List<T> list);

        /// <summary>
        /// 大数据分页写入
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="list">集合</param>
        /// <returns></returns>
        int PageBulkCopy(int pageSize, List<T> list);

        /// <summary>
        /// 大数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        int BulkUpdate(List<T> list);

        /// <summary>
        /// 海量数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        int HugeDataBulkCopy(List<T> list);

        /// <summary>
        /// 海量数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        int HugeDataBulkUpdate(List<T> list);

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
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        Task<List<T>> GetPageAsync(QueryCondition queryCondition, RefAsync<int> totalCount);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        Task<List<T>> GetListAsync(QueryCondition queryCondition);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>实体</returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        Task<List<T>> SqlQueryAsync(QueryCondition queryCondition, String sqlKey, RefAsync<int> totalCount);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录条数</param>
        /// <param name="strSQL">sql键值</param>
        /// <returns>查询结果</returns>
        Task<List<T>> SqlQueryAsync(QueryCondition queryCondition, RefAsync<int> totalCount, String strSQL);

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <param name="totalCount">记录总数</param>
        /// <returns>查询结果</returns>
        Task<List<TDTO>> SqlQueryAsync<TDTO>(QueryCondition queryCondition, String sqlKey, RefAsync<int> totalCount) where TDTO : class, new();

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录总数</param>
        /// <param name="strSQL">sql查询语句</param>
        /// <returns>查询结果</returns>
        Task<List<TDTO>> SqlQueryAsync<TDTO>(QueryCondition queryCondition, RefAsync<int> totalCount, String strSQL) where TDTO : class, new();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        Task<List<T>> GetListAsync();

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <returns>查询结果</returns>
        Task<List<TDTO>> GetListAsync<TDTO>() where TDTO : class, new();

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="create">是否赋值</param>
        /// <returns>对象</returns>
        Task<T> AddEntityAsync(T obj, bool create = true);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="create">是否赋值</param>
        /// <returns>集合</returns>
        Task<List<T>> AddListAsync(List<T> list, bool create = true);

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>对象</returns>
        Task<T> UpdateEntityAsync(T obj);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>集合</returns>
        Task<List<T>> UpdateListAsync(List<T> list);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>标志</returns>
        Task<bool> DeleteEntityAsync(string id);

        /// <summary>
        /// 逻辑批量删除
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>标志位</returns>
        Task<bool> DeleteRangeAsync(int[] ids);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>标志</returns>
        Task<bool> RemoveEntityAsync(int id);

        /// <summary>
        /// 物理批量删除
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>标志</returns>
        Task<bool> RemoveRangeAsync(int[] ids);

        /// <summary>
        /// 大数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        Task<int> BulkCopyAsync(List<T> list);

        /// <summary>
        /// 大数据分页写入
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="list">集合</param>
        /// <returns></returns>
        Task<int> PageBulkCopyAsync(int pageSize, List<T> list);

        /// <summary>
        /// 大数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        Task<int> BulkUpdateAsync(List<T> list);

        /// <summary>
        /// 海量数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        Task<int> HugeDataBulkCopyAsync(List<T> list);

        /// <summary>
        /// 海量数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        Task<int> HugeDataBulkUpdateAsync(List<T> list);

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
        Task<DbResult<TEntity>> UseTransactionAsync<TEntity>(Func<Task<TEntity>> action, Action<Exception> errorCallBack = null) where TEntity : NGAdminBaseEntity;

        /// <summary>
        /// 获取一个新的Id
        /// </summary>
        /// <returns></returns>
        string GetNewId();
    }
}
