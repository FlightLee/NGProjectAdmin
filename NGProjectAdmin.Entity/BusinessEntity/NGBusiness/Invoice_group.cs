using NGProjectAdmin.Entity.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.BusinessEntity.NGBusiness
{
    /// <summary>
    /// 发票主表
    /// </summary>
    [SugarTable("invoice_group")]
    public class Invoice_group: BaseEntity
    {

        [Navigate(NavigateType.OneToMany, nameof(File_detail.FileId))]
        public List<Invoice_detail> invoice_details { get; set; }
    }
}
