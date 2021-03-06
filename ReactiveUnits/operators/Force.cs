﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webcorp.unite
{
    public partial class Force
    {
        /// <summary>
        ///     Implements the operator /.
        /// </summary>
        /// <param name="x"> The x. </param>
        /// <param name="y"> The y. </param>
        /// <returns> The result of the operator. </returns>
        public static Pressure operator /(Force x, Area y)
        {
            return new Pressure(x.Value / y.Value);
        }

        /// <summary>
        ///     Implements the operator *.
        /// </summary>
        /// <param name="f"> The f. </param>
        /// <param name="l"> The l. </param>
        /// <returns> The result of the operator. </returns>
        public static Torque operator *(Force f, Length l)
        {
            return new Torque(f.Value * l.Value);
        }
    }
}
