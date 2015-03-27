namespace Framework.Models
{
    using System;

    using Framework.Knockout;

    [KnockoutModel]
    public class SystemLogGroupModel : EntityModel
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the machine.
        /// </summary>
        ///
        /// <value>
        ///     The name of the machine.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string MachineName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Date/Time of the timestamp.
        /// </summary>
        ///
        /// <value>
        ///     The timestamp.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DateTime? Timestamp { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the application.
        /// </summary>
        ///
        /// <value>
        ///     The name of the application.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ApplicationName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the number of no ofs.
        /// </summary>
        ///
        /// <value>
        ///     The number of no ofs.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public int NoOfCount { get; set; }
    }
}
