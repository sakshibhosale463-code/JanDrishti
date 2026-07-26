using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Domain.Common;
public partial interface IBaseEntityWithProperties
{
    /// <summary>
    /// Gets or sets the ID of the user who created the record.
    /// </summary>
    long? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was created.
    /// </summary>
    DateTime? CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was last updated, if applicable.
    /// </summary>
    DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who last updated the record, if applicable.
    /// </summary>
    long? UpdatedBy { get; set; }
}
