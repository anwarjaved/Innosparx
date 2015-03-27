using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Services.Domain
{
    public class EmailQueueItem
    {
        public Guid ID { get; set; }

        public string Message { get; set; }

        public DateTime? SendDate { get; set; }

        public DateTime CreateDate { get; set; }
        public int RetryAttempt { get; set; }

        public string ErrorMessage { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the row version.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 3:30 PM.
        /// </remarks>
        ///
        /// <value>
        ///     The row version.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public byte[] RowVersion { get; private set; }
    }
}
