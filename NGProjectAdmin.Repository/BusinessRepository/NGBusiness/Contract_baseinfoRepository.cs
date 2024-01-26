using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NGProjectAdmin.Entity.BusinessDTO.NGBusiness;
using NGProjectAdmin.Entity.BusinessEntity.BusinessModule;
using NGProjectAdmin.Entity.BusinessEntity.NGBusiness;
using NGProjectAdmin.Entity.CommonEnum;
using NGProjectAdmin.Entity.CoreEntity;
using NGProjectAdmin.Repository.Base;
using SqlSugar;
using SqlSugar.Extensions;
using StackExchange.Redis;
using System.Collections.Generic;

namespace NGProjectAdmin.Repository.BusinessRepository.NGBusiness
{


    /// <summary>
    /// File_group数据访问层实现
    /// </summary>   
    public class Contract_baseinfoRepository : BaseRepository<Contract_baseinfo>, IContract_baseinfoRepository
    {
        /// <summary>
        /// HttpContext
        /// </summary>
        private readonly IHttpContextAccessor context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public Contract_baseinfoRepository(IHttpContextAccessor context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Contract_feeinfo>> BuildContractFeeInfo(Contract_baseinfo contract_Baseinfo)
        {
            List<Contract_feeinfo> waiteFeeinfo = new List<Contract_feeinfo>();
            try
            {
                NGDbContext.BeginTran();

                // 先将收费表之前生成的未缴费明细删掉重新插
                await NGDbContext.Deleteable<Contract_feeinfo>().Where(x => x.contractId == contract_Baseinfo.Id && x.IsFee == 0).ExecuteCommandAsync();
                List<Contract_feeinfo> contract_Feeinfo = NGDbContext.Queryable<Contract_feeinfo>().Where(x => x.contractId == contract_Baseinfo.Id && x.IsFee == 1).ToList();

                int[] kk2 = toResult(contract_Baseinfo.ContractEndDate, contract_Baseinfo.ContracStartDate, diffResultFormat.yymm);
                if (contract_Baseinfo.ContractPayment == 0)
                {

                    //半年一交
                    for (int i = 0; i < kk2[0] * 2; i++)
                    {
                        Contract_feeinfo contract_Feeinfo1 = new Contract_feeinfo(this.context) { Amount = contract_Baseinfo.ContractPrice / 2, IsFee = 0, contractId = contract_Baseinfo.Id, contractBeginTime = contract_Baseinfo.ContracStartDate.AddMonths(i * 6), contractEndTime = contract_Baseinfo.ContracStartDate.AddMonths((i + 1) * 6) };

                        waiteFeeinfo.Add(contract_Feeinfo1);
                    }
                    if (kk2[1] != 0)
                    {
                        if (kk2[1] <= 6) //不满一年大于半年按一年算
                        {
                            Contract_feeinfo contract_Feeinfo1 = new Contract_feeinfo(this.context) { Amount = contract_Baseinfo.ContractPrice / 2, IsFee = 0, contractId = contract_Baseinfo.Id, contractBeginTime = new DateTime(contract_Baseinfo.ContractEndDate.Year, 1, 1), contractEndTime = contract_Baseinfo.ContractEndDate };
                            waiteFeeinfo.Add(contract_Feeinfo1);
                        }
                        else
                        {
                            Contract_feeinfo contract_Feeinfo1 = new Contract_feeinfo(this.context) { Amount = contract_Baseinfo.ContractPrice / 2, IsFee = 0, contractId = contract_Baseinfo.Id, contractBeginTime = new DateTime(contract_Baseinfo.ContractEndDate.Year, 1, 1), contractEndTime = new DateTime(contract_Baseinfo.ContractEndDate.Year, 6, 30) };
                            waiteFeeinfo.Add(contract_Feeinfo1);
                            Contract_feeinfo contract_Feeinfo2 = new Contract_feeinfo(this.context) { Amount = contract_Baseinfo.ContractPrice / 2, IsFee = 0, contractId = contract_Baseinfo.Id, contractBeginTime = new DateTime(contract_Baseinfo.ContractEndDate.Year, 7, 1), contractEndTime = contract_Baseinfo.ContractEndDate };
                            waiteFeeinfo.Add(contract_Feeinfo2);
                        }
                    }

                }
                else
                {
                    //一年一交
                    for (int i = 0; i < kk2[0]; i++)
                    {
                        Contract_feeinfo contract_Feeinfo1 = new Contract_feeinfo(this.context) { Amount = contract_Baseinfo.ContractPrice, IsFee = 0, contractId = contract_Baseinfo.Id, contractBeginTime = contract_Baseinfo.ContracStartDate.AddYears(i), contractEndTime = contract_Baseinfo.ContracStartDate.AddYears(i + 1) };
                        waiteFeeinfo.Add(contract_Feeinfo1);
                    }
                    if (kk2[1] != 0)
                    {
                        Contract_feeinfo contract_Feeinfo1 = new Contract_feeinfo(this.context) { Amount = contract_Baseinfo.ContractPrice, IsFee = 0, contractId = contract_Baseinfo.Id, contractBeginTime = contract_Baseinfo.ContracStartDate.AddYears(kk2[0]).AddDays(1), contractEndTime = contract_Baseinfo.ContractEndDate };
                        waiteFeeinfo.Add(contract_Feeinfo1);
                    }
                }
                List<Contract_feeinfo> waiteFeeinfo2 = new List<Contract_feeinfo>();
                if (contract_Feeinfo != null && contract_Feeinfo.Count > 0)
                {

                    //如果之前有缴费了,生成的待缴费明细把缴过的去除掉
                    foreach (Contract_feeinfo item in waiteFeeinfo)
                    {
                        if (contract_Feeinfo.Find(x => x.contractBeginTime == item.contractBeginTime && x.contractEndTime == item.contractEndTime) == null)
                        {
                            waiteFeeinfo2.Add(item);
                        }
                    }
                    waiteFeeinfo = waiteFeeinfo2;
                }
                await NGDbContext.Insertable(waiteFeeinfo).ExecuteCommandAsync();

                NGDbContext.CommitTran();
            }
            catch (Exception)
            {

                throw;
            }
            return waiteFeeinfo;

        }

        /// <summary>
        /// 计算日期间隔
        /// </summary>
        /// <param name="d1">要参与计算的其中一个日期</param>
        /// <param name="d2">要参与计算的另一个日期</param>
        /// <param name="drf">决定返回值形式的枚举</param>
        /// <returns>一个代表年月日的int数组，具体数组长度与枚举参数drf有关</returns>
        public static int[] toResult(DateTime d1, DateTime d2, diffResultFormat drf)
        {
            #region 数据初始化
            DateTime max;
            DateTime min;
            int year;
            int month;
            int tempYear, tempMonth;
            if (d1 > d2)
            {
                max = d1;
                min = d2;
            }
            else
            {
                max = d2;
                min = d1;
            }
            tempYear = max.Year;
            tempMonth = max.Month;
            if (max.Month < min.Month)
            {
                tempYear--;
                tempMonth = tempMonth + 12;
            }
            year = tempYear - min.Year;
            month = tempMonth - min.Month;
            #endregion
            #region 按条件计算
            if (drf == diffResultFormat.dd)
            {
                TimeSpan ts = max - min;
                return new int[] { ts.Days };
            }
            if (drf == diffResultFormat.mm)
            {
                return new int[] { month + year * 12 };
            }
            if (drf == diffResultFormat.yy)
            {
                return new int[] { year };
            }
            return new int[] { year, month };
            #endregion
        }

        public async Task<List<Contract_baseinfo>> GetContracts(QueryCondition queryCondition, RefAsync<int> totalCount)
        {
            var where = QueryCondition.BuildExpression<Contract_baseinfo>(queryCondition.QueryItems);
            List<Contract_baseinfo> list = await NGDbContext.Queryable<Contract_baseinfo>().WhereIF(true, where).Where(x => x.IsDel == 0).ToPageListAsync(queryCondition.PageIndex, queryCondition.PageSize, totalCount);
            return list;
        }

        public async Task<Contract_baseinfoDTO> GetById(Contract_baseinfoDTO contract_Baseinfo)
        {
            var info = await NGDbContext.Queryable<Contract_baseinfo>()
                .LeftJoin<Assets_info>((a,c)=>a.AssetsId==c.Id && a.IsDel == 0 && c.IsDel == 0)
                .Where(a=>a.Id== contract_Baseinfo.Id)
                .Select((a,c)=>new Contract_baseinfoDTO() { 
                    Id=a.Id,
                    AssetsAdress=c.AssetsAdress,
                    ContractDate=a.ContractDate,
                    ContractType=a.ContractType,
                    ContracStartDate=a.ContracStartDate,
                    ContractEndDate=a.ContractEndDate,
                    ContractLife= a.ContractLife.ToString(),
                    ContractPromiseMoney=a.ContractPromiseMoney.ToString(),
                    ContractPrice=a.ContractPrice.ToString(),
                    ContractMoney=a.ContractMoney.ToString(),
                    ContractPayment=a.ContractPayment,
                    ContractState=a.ContractState,
                    Remark=a.Remark,
                    Lessor = a.Lessor,
                    lessee=a.lessee,
                    lessorPhone = a.lessorPhone ,
                    lesseePhone=a.lesseePhone,
                    LessorAdress=a.LessorAdress,
                    lesseeAdress=a.lesseeAdress,
                    lessorId=a.lessorId,
                    lesseeId = a.lesseeId,
                    ContractPdfGroupId=a.ContractPdfGroupId,
                    AssetsId=c.Id,
                    assetsName=c.assetsName
                })
                .FirstAsync();

            return info;


        }

        public async Task<List<AssetsSelect>> GetAllAssets()
        {
            var list = await NGDbContext.Queryable<Assets_info>().Where(x => x.IsDel == 0).Select((x)=>new AssetsSelect() {  AssetsCode= x.Id, AssetsName=x.assetsName }).ToListAsync();
          
            return list;
        }
    }
}
