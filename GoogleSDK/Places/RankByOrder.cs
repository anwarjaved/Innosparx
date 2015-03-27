using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Places
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Specifies the order in which results are listed.
    /// </summary>
    ///
    /// <remarks>
    ///     LM ANWAR, 4/22/2013.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public enum RankByOrder
    {
        /// <summary>
        ///     This option sorts results based on their importance.
        /// </summary>
        Prominence,

        /// <summary>
        ///     This option sorts results in ascending order by their distance from the specified location.
        /// </summary>
        Distance
    }
}
