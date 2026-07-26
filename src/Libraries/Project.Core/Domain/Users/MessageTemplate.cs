using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Domain.Localization;

namespace Project.Core.Domain.Users;
public class MessageTemplate : BaseEntity, ILocalizedEntity
{
    public string Name { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public bool IsActive { get; set; }
}
