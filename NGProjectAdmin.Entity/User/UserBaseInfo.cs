using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGProjectAdmin.Entity.User
{
    public class UserBaseInfo
    {
        [SugarColumn(IsPrimaryKey = true, OracleSequenceName = "Seq_Id")]
        public int Id { get; set; }
    }
}
