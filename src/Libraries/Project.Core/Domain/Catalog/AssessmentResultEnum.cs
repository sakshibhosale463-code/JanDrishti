using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Domain.Catalog;
public enum AssessmentResultEnum
{
    /// <summary>
    /// Attempt is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Attempt has been submitted.
    /// </summary>
    Submitted = 2,

    /// <summary>
    /// Student has passed the assessment.
    /// </summary>
    Pass = 3,

    /// <summary>
    /// Student has failed the assessment.
    /// </summary>
    Fail = 4
}
