namespace Framework.Models
{
    using System.ComponentModel;
    using System.Net;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Base model.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class BaseModel
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the BaseModel class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public BaseModel()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the BaseModel class.
        /// </summary>
        ///
        /// <param name="errorMessage">
        ///     Message describing the error.
        /// </param>
        /// <param name="errorCode">
        ///     (optional) the error code.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public BaseModel(string errorMessage, HttpStatusCode? errorCode = null)
        {
            this.ErrorMessage = errorMessage;
            this.StatusCode = errorCode;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the status code.
        /// </summary>
        ///
        /// <value>
        ///     The status code.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [Ignore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public HttpStatusCode? StatusCode { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a message describing the error.
        /// </summary>
        ///
        /// <value>
        ///     A message describing the error.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [Ignore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public string ErrorMessage { get; set; }
    }
}
