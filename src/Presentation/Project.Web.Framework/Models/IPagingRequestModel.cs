using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Web.Framework.Models
{
    public partial interface IPagingRequestModel
    {
        /// <summary>
        /// Gets a page number
        /// </summary>
        int Page { get; }

        /// <summary>
        /// Gets a page size
        /// </summary>
        int PageSize { get; }
    }
}
