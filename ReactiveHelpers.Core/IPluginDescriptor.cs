using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveHelpers.Core
{
    public interface IPluginDescriptor
    {
        /// <summary>
        /// Unique name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Display Name
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The author name
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Version compliance of this plugin
        /// </summary>
        VersionCompliance Version { get; }

        /// <summary>
        /// Little description of plugin
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Status
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// Message display before uninstall a plugin
        /// </summary>
        string DisplayBeforeUninstall { get; }

        
    }
}
