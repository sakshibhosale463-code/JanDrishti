using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core;

namespace Project.Services.Report.EntityModel;
public class AttendanceEntityModel : BaseEntity
{
    public long TotalAbsent { get; set; }

    public long TotalPresent { get; set; }
}
