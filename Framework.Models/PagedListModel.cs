using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Models
{
    using Framework.Knockout;
    using Framework.Paging;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A data Model for the paged list.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 03/05/2014 1:27 PM.
    /// </remarks>
    ///
    /// <typeparam name="T">
    ///     Generic type parameter.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    [KnockoutModel]
    public class PagedListModel<T> : BaseModel
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the PagedListModel class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 1:27 PM.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------
        public PagedListModel()
        {
            this.Items = new List<T>();
            this.PageIndex = 0;
            this.PageSize = 10;
            this.TotalCount = 0;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the PagedListModel class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 1:28 PM.
        /// </remarks>
        ///
        /// <param name="items">
        ///     The items.
        /// </param>
        /// <param name="pageIndex">
        ///     Zero-based index of this subset within the superset.
        /// </param>
        /// <param name="pageSize">
        ///     Maximum size any individual subset.
        /// </param>
        /// <param name="totalCount">
        ///     Total number of objects contained within the superset.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public PagedListModel(IEnumerable<T> items, int pageIndex, int pageSize, int totalCount)
        {
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.Items = items;
            this.PageIndex = pageIndex;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the PagedListModel class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 1:28 PM.
        /// </remarks>
        ///
        /// <param name="items">
        ///     The items.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public PagedListModel(IPagedList<T> items)
        {
            this.Items = items;
            this.TotalCount = items.TotalCount;
            this.PageIndex = items.PageIndex;
            this.PageSize = items.PageSize;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the items.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 1:28 PM.
        /// </remarks>
        ///
        /// <value>
        ///     The items.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public IEnumerable<T> Items { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets Total number of objects contained within the superset.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 1:28 PM.
        /// </remarks>
        ///
        /// <value>
        ///     Total number of objects contained within the superset.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public int TotalCount { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets Zero-based index of this subset within the superset.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 1:28 PM.
        /// </remarks>
        ///
        /// <value>
        ///     Zero-based index of this subset within the superset.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public int PageIndex { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets Maximum size any individual subset.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/05/2014 1:28 PM.
        /// </remarks>
        ///
        /// <value>
        ///     Maximum size any individual subset.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public int PageSize { get; private set; }
    }
}
