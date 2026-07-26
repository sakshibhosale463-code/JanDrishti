using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Configuration;
public class SqsOptions:IConfig
{
    public string SqsRegion { get; set; }
    public string SqsQueueId { get; set; }
    public string SqsQueueName { get; set; }
    public string IamAccessKey { get; set; }
    public string IamSecretKey { get; set; }

    public string SqsQueuePosPush { get; set; }
    public string SqsQueuePosPull { get; set; }

    public string SqsQueueEcomPush { get; set; }
    public string SqsQueueEcomPull { get; set; }

    public string SqsQueueFix360Push { get; set; }
    public string SqsQueueDailyXPush { get; set; }
}
