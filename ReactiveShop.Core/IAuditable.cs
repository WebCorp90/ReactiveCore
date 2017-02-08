using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveShop.Core
{
    public interface IAuditable
    {
        /// <summary>
		/// Gets or sets the date and time of entity creation
		/// </summary>
		DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity update
        /// </summary>
        DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the identy user of entity creation
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the identy user of entity update
        /// </summary>
        string UpdatedBy { get; set; }
    }
}
