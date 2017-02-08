using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveShop.Core
{
    /// <summary>
    /// Make abillity to mark object as Soft Deletable
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// Mark as Soft Deleted
        /// </summary>
        bool MarkDeleted { get; set; }
    }
}
