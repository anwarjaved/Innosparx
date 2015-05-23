namespace Framework.Models
{
    using System.Security;

    
    public class DataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        public string Echo { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int DisplayStart { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int PageNumber { get; set; }


        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int NoofColumns { get; set; }

        /// <summary>
        /// Name of columns that are used in sorting
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        /// Name of columns that are used in sorting
        /// </summary>
        public string SortDirection { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string[] Columns { get; set; }
    }
}
