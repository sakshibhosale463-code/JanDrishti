using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Domain.Common;
public class BillSetting : BaseEntity
{
    public string Prefix { get; set; }
    public int SerialNo { get; set; }
    public string Initial { get; set; }
    public string Year { get; set; }
    public bool Deleted { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

