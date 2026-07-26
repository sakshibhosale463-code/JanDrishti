using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Web.Framework.Models
{
    /// <summary>
    /// Represents a paged model
    /// </summary>
    public partial interface IPagedModel<T> where T : BaseNopModel
    {
    }
}