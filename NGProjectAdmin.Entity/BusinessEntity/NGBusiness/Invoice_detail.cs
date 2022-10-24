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
    /// 发票明细
    /// </summary>
    [SugarTable("invoice_detail")]
    public class Invoice_detail:BaseEntity
    {
        public int InvoiceId { get; set; }

        public int insideId { get; set; }

        public DateTime BuildDate { get; set; }
        public double Money { get; set; }
        public int InvoiceType { get; set; }
        public string InvoicePerson { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 发票图片Id
        /// </summary>
        public int InvoiceFileId { get; set; }

        /// <summary>
        /// 评估文件
        /// </summary>
        [Navigate(NavigateType.OneToOne, nameof(InvoiceFileId), nameof(File_group.Id))]
        public File_group? InvoiceFiles { get; set; }
    }
}
