using Microsoft.AspNetCore.Http;
using NGProjectAdmin.Common.Class.Exceptions;
using NGProjectAdmin.Common.Global;
using NGProjectAdmin.Entity.Base;
using NGProjectAdmin.Entity.BusinessEntity.SystemManagement;
using NGProjectAdmin.Entity.CoreEntity;

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NGProjectAdmin.Repository.Base
{
    /// <summary>
    /// 数据库访问基类
    /// </summary>
    public class BaseRepository<T> : NGAdminDbScope, IBaseRepository<T> where T :BaseEntity, new()
    {
        #region 属性及构造函数

        /// <summary>
        /// 数据库上下文
        /// </summary>
        //private readonly Repository<T> Repository;

        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public BaseRepository(IHttpContextAccessor context)
        {
            //this.Repository = new Repository<T>();
            this.context = context;
        }

        #endregion

        #region 基类公有方法

        #region 基类同步方法

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        public List<T> GetPage(QueryCondition queryCondition, ref int totalCount)
        {
            QueryCondition.AddDefaultQueryItem(queryCondition);

            var where = QueryCondition.BuildExpression<T>(queryCondition.QueryItems);

            var list = NGDbContext.
                Queryable<T>().
                WhereIF(true, where).
                OrderByIF(!String.IsNullOrEmpty(queryCondition.Sort), queryCondition.Sort).
                ToPageList(queryCondition.PageIndex, queryCondition.PageSize, ref totalCount).
                ToList();

            return list;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public List<T> GetList(QueryCondition queryCondition)
        {
            QueryCondition.AddDefaultQueryItem(queryCondition);

            var where = QueryCondition.BuildExpression<T>(queryCondition.QueryItems);

            var list = NGDbContext.
                Queryable<T>().
                WhereIF(true, where).
                OrderByIF(!String.IsNullOrEmpty(queryCondition.Sort), queryCondition.Sort).
                ToList();

            return list;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>实体</returns>
        public T GetById(int id)
        {
            return NGDbContext.Queryable<T>().First(t => t.Id.Equals(id));
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        public List<T> SqlQuery(QueryCondition queryCondition, String sqlKey, ref int totalCount)
        {
            var sqlStr = this.GetQuerySQL(queryCondition, sqlKey);

            var list = this.GetPageList(queryCondition, sqlStr, ref totalCount);

            return list;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录条数</param>
        /// <param name="strSQL">查询语句</param>
        /// <returns>查询结果</returns>
        public List<T> SqlQuery(QueryCondition queryCondition, ref int totalCount, String strSQL)
        {
            var baseSQL = this.GetBaseSQL();
            baseSQL = String.Format(baseSQL, strSQL);

            var condition = this.ConvertQueryCondition(queryCondition.QueryItems);
            var sort = this.ConvertSort(queryCondition.Sort);

            var sqlStr = String.Join("", baseSQL, condition, sort);

            var list = this.GetPageList(queryCondition, sqlStr, ref totalCount);

            return list;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <param name="totalCount">记录总数</param>
        /// <returns>查询结果</returns>
        public List<TDTO> SqlQuery<TDTO>(QueryCondition queryCondition, String sqlKey, ref int totalCount) where TDTO : class, new()
        {
            var sqlStr = this.GetQuerySQL(queryCondition, sqlKey);

            var list = this.GetPageList<TDTO>(queryCondition, sqlStr, ref totalCount);

            return list;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录总数</param>
        /// <param name="strSQL">查询语句</param>
        /// <returns>查询结果</returns>
        public List<TDTO> SqlQuery<TDTO>(QueryCondition queryCondition, ref int totalCount, String strSQL) where TDTO : class, new()
        {
            var baseSQL = this.GetBaseSQL();
            baseSQL = String.Format(baseSQL, strSQL);

            var condition = this.ConvertQueryCondition(queryCondition.QueryItems);
            var sort = this.ConvertSort(queryCondition.Sort);

            var sqlStr = String.Join("", baseSQL, condition, sort);

            var list = this.GetPageList<TDTO>(queryCondition, sqlStr, ref totalCount);

            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        public List<T> GetList()
        {
            return NGDbContext.Queryable<T>().ToList();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <returns>查询结果</returns>
        public List<TDTO> GetList<TDTO>() where TDTO : class, new()
        {
            return NGDbContext.Queryable<TDTO>().ToList();
        }

        /// <summary>
        /// 按表达式查询
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>列表</returns>
        public List<T> QueryByExpression(Expression<Func<T, bool>> expression)
        {
            return NGDbContext.Queryable<T>().Where(expression).ToList();
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="create">是否赋值</param>
        /// <returns>对象</returns>
        public T AddEntity(T obj, bool create = true)
        {
            //判断审计日志记录开关是否开启
            if (obj.GetType().Equals(typeof(SysLog)))
            {
                if (!NGAdminGlobalContext.LogConfig.IsEnabled)
                {
                    return obj;
                }
            }

            if (create)
            {
                obj.Create(this.context);
            }

            try
            {
                NGDbContext.BeginTran();
                NGDbContext.Insertable<T>(obj).ExecuteCommand();
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return obj;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="create">是否赋值</param>
        /// <returns>集合</returns>
        public List<T> AddList(List<T> list, bool create = true)
        {
            //判断审计日志记录开关是否开启
            if (list != null && list.Count > 0 && list.FirstOrDefault().GetType().Equals(typeof(SysLog)))
            {
                if (!NGAdminGlobalContext.LogConfig.IsEnabled)
                {
                    return list;
                }
            }

            if (create)
            {
                foreach (var item in list)
                {
                    item.Create(this.context);
                }
            }

            try
            {
                NGDbContext.BeginTran();
                NGDbContext.Insertable<T>(list).ExecuteCommand();
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return list;
        }

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>对象</returns>
        public T UpdateEntity(T obj)
        {
            var entity = NGDbContext.Queryable<T>().WhereClassByPrimaryKey(obj).ToList().FirstOrDefault();
     
            obj.Modify(this.context);

            try
            {
                NGDbContext.BeginTran();
                NGDbContext.Updateable<T>(obj).ExecuteCommand();
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return obj;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>集合</returns>
        public List<T> UpdateList(List<T> list)
        {
            foreach (var item in list)
            {
                var entity = NGDbContext.Queryable<T>().WhereClassByPrimaryKey(item).ToList().FirstOrDefault();
              
                item.Modify(this.context);
            }

            try
            {
                NGDbContext.BeginTran();
                NGDbContext.Updateable<T>(list).ExecuteCommand();
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return list;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>标志</returns>
        public bool DeleteEntity(int id)
        {
            var result = false;

            var entity = NGDbContext.Queryable<T>().First(t => t.Id.Equals(id));
            entity.Delete(this.context);

            try
            {
                NGDbContext.BeginTran();
                result = NGDbContext.Updateable<T>(entity).ExecuteCommand() > 0;
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 逻辑批量删除
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>标志位</returns>
        public bool DeleteRange(int[] ids)
        {
            var result = false;

            var list = new List<T>();
            foreach (var item in ids)
            {
                var entity = NGDbContext.Queryable<T>().First(t => t.Id.Equals(item));
                entity.Delete(this.context);
                list.Add(entity);
            }

            try
            {
                NGDbContext.BeginTran();
                result = NGDbContext.Updateable<T>(list).ExecuteCommand() > 0;
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>标志</returns>
        public bool RemoveEntity(int id)
        {
            var result = false;

            try
            {
                NGDbContext.BeginTran();
                result = NGDbContext.Deleteable<T>().In(id).ExecuteCommand() > 0;
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 物理批量删除
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>标志</returns>
        public bool RemoveRange(int[] ids)
        {
            var result = false;

            try
            {
                NGDbContext.BeginTran();
                result = NGDbContext.Deleteable<T>().In(ids).ExecuteCommand() > 0;
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 大数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public int BulkCopy(List<T> list)
        {
            return NGDbContext.Fastest<T>().BulkCopy(list);
        }

        /// <summary>
        /// 大数据分页写入
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public int PageBulkCopy(int pageSize, List<T> list)
        {
            return NGDbContext.Fastest<T>().PageSize(pageSize).BulkCopy(list);
        }

        /// <summary>
        /// 大数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public int BulkUpdate(List<T> list)
        {
            return NGDbContext.Fastest<T>().BulkUpdate(list);
        }

        /// <summary>
        /// 海量数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public int HugeDataBulkCopy(List<T> list)
        {
            var hugeData = NGDbContext.Storageable<T>(list).ToStorage();
            return hugeData.BulkCopy();
        }

        /// <summary>
        /// 海量数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public int HugeDataBulkUpdate(List<T> list)
        {
            var hugeData = NGDbContext.Storageable<T>(list).ToStorage();
            return hugeData.BulkUpdate();
        }

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        public DbResult<bool> UseTransaction(Action action, Action<Exception> errorCallBack = null)
        {
            return NGDbContext.AsTenant().UseTran(action, errorCallBack);
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
            return NGDbContext.AsTenant().UseTran(action, errorCallBack);
        }

        /// <summary>
        /// 时间版本比较
        /// </summary>
        /// <param name="entity">数据库实体</param>
        /// <param name="obj">业务实体</param>
        /// <returns>真假值</returns>

        #endregion

        #region 基类异步方法

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        public async Task<List<T>> GetPageAsync(QueryCondition queryCondition, RefAsync<int> totalCount)
        {
            QueryCondition.AddDefaultQueryItem(queryCondition);

            var where = QueryCondition.BuildExpression<T>(queryCondition.QueryItems);

            var list = await NGDbContext.
                Queryable<T>().
                WhereIF(true, where).
                OrderByIF(!String.IsNullOrEmpty(queryCondition.Sort), queryCondition.Sort).
                ToPageListAsync(queryCondition.PageIndex, queryCondition.PageSize, totalCount);

            return list;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns>查询结果</returns>
        public async Task<List<T>> GetListAsync(QueryCondition queryCondition)
        {
            QueryCondition.AddDefaultQueryItem(queryCondition);

            var where = QueryCondition.BuildExpression<T>(queryCondition.QueryItems);

            var list = await NGDbContext.
                Queryable<T>().
                WhereIF(true, where).
                OrderByIF(!String.IsNullOrEmpty(queryCondition.Sort), queryCondition.Sort).
                ToListAsync();

            return list;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>实体</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await NGDbContext.Queryable<T>().FirstAsync(t => t.Id.Equals(id));
        }


        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        public async Task<List<T>> SqlQueryAsync(QueryCondition queryCondition, String sqlKey, RefAsync<int> totalCount)
        {
            var sqlStr = this.GetQuerySQL(queryCondition, sqlKey);

            var list = await this.GetPageListAsync(queryCondition, sqlStr, totalCount);

            return list;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录条数</param>
        /// <param name="strSQL">查询语句</param>
        /// <returns>查询结果</returns>
        public async Task<List<T>> SqlQueryAsync(QueryCondition queryCondition, RefAsync<int> totalCount, String strSQL)
        {
            var baseSQL = this.GetBaseSQL();
            baseSQL = String.Format(baseSQL, strSQL);

            var condition = this.ConvertQueryCondition(queryCondition.QueryItems);
            var sort = this.ConvertSort(queryCondition.Sort);

            var sqlStr = String.Join("", baseSQL, condition, sort);

            var list = await this.GetPageListAsync(queryCondition, sqlStr, totalCount);

            return list;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <param name="totalCount">记录总数</param>
        /// <returns>查询结果</returns>
        public async Task<List<TDTO>> SqlQueryAsync<TDTO>(QueryCondition queryCondition, String sqlKey, RefAsync<int> totalCount) where TDTO : class, new()
        {
            var sqlStr = this.GetQuerySQL(queryCondition, sqlKey);

            var list = await this.GetPageListAsync<TDTO>(queryCondition, sqlStr, totalCount);

            return list;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="totalCount">记录总数</param>
        /// <param name="strSQL">sql查询语句</param>
        /// <returns>查询结果</returns>
        public async Task<List<TDTO>> SqlQueryAsync<TDTO>(QueryCondition queryCondition, RefAsync<int> totalCount, String strSQL) where TDTO : class, new()
        {
            var baseSQL = this.GetBaseSQL();
            baseSQL = String.Format(baseSQL, strSQL);

            var condition = this.ConvertQueryCondition(queryCondition.QueryItems);
            var sort = this.ConvertSort(queryCondition.Sort);

            var sqlStr = String.Join("", baseSQL, condition, sort);

            var list = await this.GetPageListAsync<TDTO>(queryCondition, sqlStr, totalCount);

            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        public async Task<List<T>> GetListAsync()
        {
            return await NGDbContext.Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <returns>查询结果</returns>
        public async Task<List<TDTO>> GetListAsync<TDTO>() where TDTO : class, new()
        {
            return await NGDbContext.Queryable<TDTO>().ToListAsync();
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="create">是否赋值</param>
        /// <returns>对象</returns>
        public async Task<T> AddEntityAsync(T obj, bool create = true)
        {
            //判断审计日志记录开关是否开启
            if (obj.GetType().Equals(typeof(SysLog)))
            {
                if (!NGAdminGlobalContext.LogConfig.IsEnabled)
                {
                    return obj;
                }
            }

            if (create)
            {
                obj.Create(this.context);
            }

            try
            {
                NGDbContext.BeginTran();
                await NGDbContext.Insertable<T>(obj).ExecuteCommandAsync();
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return obj;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="create">是否赋值</param>
        /// <returns>集合</returns>
        public async Task<List<T>> AddListAsync(List<T> list, bool create = true)
        {
            //判断审计日志记录开关是否开启
            if (list.FirstOrDefault().GetType().Equals(typeof(SysLog)))
            {
                if (!NGAdminGlobalContext.LogConfig.IsEnabled)
                {
                    return list;
                }
            }

            if (create)
            {
                foreach (var item in list)
                {
                    item.Create(this.context);
                }
            }

            try
            {
                NGDbContext.BeginTran();
                await NGDbContext.Insertable<T>(list).ExecuteCommandAsync();
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return list;
        }

        /// <summary>
        /// 编辑对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>对象</returns>
        public async Task<T> UpdateEntityAsync(T obj)
        {
            var entity = NGDbContext.Queryable<T>().WhereClassByPrimaryKey(obj).ToList().FirstOrDefault();
         
            obj.Modify(this.context);

            try
            {
                NGDbContext.BeginTran();
                await NGDbContext.Updateable<T>(obj).ExecuteCommandAsync();
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return obj;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns>集合</returns>
        public async Task<List<T>> UpdateListAsync(List<T> list)
        {
            foreach (var item in list)
            {
                var entity = NGDbContext.Queryable<T>().WhereClassByPrimaryKey(item).ToList().FirstOrDefault();               
                item.Modify(this.context);
            }

            try
            {
                NGDbContext.BeginTran();
                await NGDbContext.Updateable<T>(list).ExecuteCommandAsync();
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return list;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>标志</returns>
        public async Task<bool> DeleteEntityAsync(int id)
        {
            var result = false;

            var entity = NGDbContext.Queryable<T>().First(t => t.Id.Equals(id));
            entity.Delete(this.context);

            try
            {
                NGDbContext.BeginTran();
                result = await NGDbContext.Updateable<T>(entity).ExecuteCommandAsync() > 0;
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 逻辑批量删除
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>标志位</returns>
        public async Task<bool> DeleteRangeAsync(int[] ids)
        {
            var result = false;

            var list = new List<T>();
            foreach (var item in ids)
            {
                var entity = NGDbContext.Queryable<T>().First(t => t.Id.Equals(item));
                entity.Delete(this.context);
                list.Add(entity);
            }

            try
            {
                NGDbContext.BeginTran();
                result = await NGDbContext.Updateable<T>(list).ExecuteCommandAsync() > 0;
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>标志</returns>
        public async Task<bool> RemoveEntityAsync(int id)
        {
            var result = false;

            try
            {
                NGDbContext.BeginTran();
                result = await NGDbContext.Deleteable<T>().In(id).ExecuteCommandAsync() > 0;
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 物理批量删除
        /// </summary>
        /// <param name="ids">编号组</param>
        /// <returns>标志</returns>
        public async Task<bool> RemoveRangeAsync(int[] ids)
        {
            var result = false;

            try
            {
                NGDbContext.BeginTran();
                result = await NGDbContext.Deleteable<T>().In(ids).ExecuteCommandAsync() > 0;
                NGDbContext.CommitTran();
            }
            catch (Exception ex)
            {
                NGDbContext.RollbackTran();
                throw new NGAdminCustomException(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 大数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public async Task<int> BulkCopyAsync(List<T> list)
        {
            return await NGDbContext.Fastest<T>().BulkCopyAsync(list);
        }

        /// <summary>
        /// 大数据分页写入
        /// </summary>
        /// <param name="pageSize">页数</param>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public async Task<int> PageBulkCopyAsync(int pageSize, List<T> list)
        {
            return await NGDbContext.Fastest<T>().PageSize(pageSize).BulkCopyAsync(list);
        }

        /// <summary>
        /// 大数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public async Task<int> BulkUpdateAsync(List<T> list)
        {
            return await NGDbContext.Fastest<T>().BulkUpdateAsync(list);
        }

        /// <summary>
        /// 海量数据写入
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public async Task<int> HugeDataBulkCopyAsync(List<T> list)
        {
            var hugeData = NGDbContext.Storageable<T>(list).ToStorage();
            return await hugeData.BulkCopyAsync();
        }

        /// <summary>
        /// 海量数据更新
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public async Task<int> HugeDataBulkUpdateAsync(List<T> list)
        {
            var hugeData = NGDbContext.Storageable<T>(list).ToStorage();
            return await hugeData.BulkUpdateAsync();
        }

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="action">委托事件</param>
        /// <param name="errorCallBack">错误回调事件</param>
        /// <returns>DbResult</returns>
        public async Task<DbResult<bool>> UseTransactionAsync(Func<Task> action, Action<Exception> errorCallBack = null)
        {
            return await NGDbContext.AsTenant().UseTranAsync(action, errorCallBack);
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
            return await NGDbContext.AsTenant().UseTranAsync(action, errorCallBack);
        }

        #endregion

        #endregion

        #region 基类私有方法        

        /// <summary>
        /// 获取基本语句
        /// </summary>
        /// <returns></returns>
        private String GetBaseSQL()
        {
            var baseSQL = NGAdminGlobalContext.Configuration.GetSection("sqls:sql:basequerysql").Value;

            if ((DbType)NGAdminGlobalContext.DBConfig.DBType == DbType.SqlServer)
            {
                baseSQL = baseSQL.Replace("*", "TOP 100 percent *");
            }

            return baseSQL;
        }

        /// <summary>
        /// 获取查询脚本
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlKey">sql键值</param>
        /// <returns>sql语句</returns>
        private String GetQuerySQL(QueryCondition queryCondition, String sqlKey)
        {
            var baseSQL = this.GetBaseSQL();

            var sqlStr = NGAdminGlobalContext.Configuration.GetSection(sqlKey).Value;

            baseSQL = String.Format(baseSQL, sqlStr);

            var condition = this.ConvertQueryCondition(queryCondition.QueryItems);

            var sort = this.ConvertSort(queryCondition.Sort);

            return String.Join("", baseSQL, condition, sort);
        }

        /// <summary>
        /// 转化查询条件
        /// </summary>
        /// <param name="queryItems">查询条件</param>
        /// <returns>查询语句</returns>
        private String ConvertQueryCondition(List<QueryItem> queryItems)
        {
            var queryCondition = String.Empty;
            if (queryItems != null && queryItems.Count > 0)
            {
                queryCondition = QueryCondition.ConvertToSQL(queryItems);
            }
            return queryCondition;
        }

        /// <summary>
        /// 转化排序条件
        /// </summary>
        /// <param name="sort">排序条件</param>
        /// <returns>排序语句</returns>
        private String ConvertSort(String sort)
        {
            var sortStr = String.Empty;
            if (!String.IsNullOrEmpty(sort))
            {
                sortStr = " ORDER BY " + sort;
            }
            return sortStr;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="strSQL">查询语句</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询集合</returns>
        private List<T> GetPageList(QueryCondition queryCondition, String strSQL, ref int totalCount)
        {
            var list = new List<T>();
            if (queryCondition.PageIndex >= 0 && queryCondition.PageSize > 0)
            {
                list = NGDbContext.SqlQueryable<T>(strSQL).
                ToPageList(queryCondition.PageIndex, queryCondition.PageSize, ref totalCount);
            }
            else
            {
                list = NGDbContext.SqlQueryable<T>(strSQL).ToList();
                totalCount = list.Count;
            }
            return list;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">TDTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlStr">查询语句</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        private List<TDTO> GetPageList<TDTO>(QueryCondition queryCondition, String sqlStr, ref int totalCount) where TDTO : class, new()
        {
            var list = new List<TDTO>();
            if (queryCondition.PageIndex >= 0 && queryCondition.PageSize > 0)
            {
                list = NGDbContext.SqlQueryable<TDTO>(sqlStr).
                ToPageList(queryCondition.PageIndex, queryCondition.PageSize, ref totalCount);
            }
            else
            {
                list = NGDbContext.SqlQueryable<TDTO>(sqlStr).ToList();
                totalCount = list.Count;
            }
            return list;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="totalCount">记录条数</param>
        /// <returns>查询结果</returns>
        private async Task<List<T>> GetPageListAsync(QueryCondition queryCondition, String sqlStr, RefAsync<int> totalCount)
        {
            var list = new List<T>();
            if (queryCondition.PageIndex >= 0 && queryCondition.PageSize > 0)
            {
                list = await NGDbContext.SqlQueryable<T>(sqlStr).
                ToPageListAsync(queryCondition.PageIndex, queryCondition.PageSize, totalCount);
            }
            else
            {
                list = await NGDbContext.SqlQueryable<T>(sqlStr).ToListAsync();
                totalCount = list.Count;
            }
            return list;
        }

        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TDTO">DTO</typeparam>
        /// <param name="queryCondition">查询条件</param>
        /// <param name="sqlStr">查询语句</param>
        /// <param name="totalCount">记录总数</param>
        /// <returns>查询结果</returns>
        private async Task<List<TDTO>> GetPageListAsync<TDTO>(QueryCondition queryCondition, String sqlStr, RefAsync<int> totalCount) where TDTO : class, new()
        {
            var list = new List<TDTO>();
            if (queryCondition.PageIndex >= 0 && queryCondition.PageSize > 0)
            {
                list = await NGDbContext.SqlQueryable<TDTO>(sqlStr).
                ToPageListAsync(queryCondition.PageIndex, queryCondition.PageSize, totalCount);
            }
            else
            {
                list = await NGDbContext.SqlQueryable<TDTO>(sqlStr).ToListAsync();
                totalCount = list.Count;
            }
            return list;
        }

        #endregion
    }
}
