namespace Framework.Rest
{
    using System.Threading.Tasks;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for rest client.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    internal interface IRestClient
    {

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Get Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        RestResponse Get(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Get Request.
        /// </summary>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        RestResponse<T> Get<T>(RestRequest request) where T : new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Post Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        RestResponse Post(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Post Request.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        RestResponse<T> Post<T>(RestRequest request) where T : new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Get Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        RestResponse Delete(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Delete Request.
        /// </summary>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        RestResponse<T> Delete<T>(RestRequest request) where T : new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Put Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        RestResponse Put(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Put Request.
        /// </summary>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        RestResponse<T> Put<T>(RestRequest request) where T : new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Head Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        RestResponse Head(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Head Request.
        /// </summary>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        RestResponse<T> Head<T>(RestRequest request) where T : new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Get Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Task<RestResponse> GetAsync(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Get Request.
        /// </summary>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        Task<RestResponse<T>> GetAsync<T>(RestRequest request) where T : new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Post Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Task<RestResponse> PostAsync(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Post Request.
        /// </summary>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        Task<RestResponse<T>> PostAsync<T>(RestRequest request) where T : new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Put Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Task<RestResponse> PutAsync(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Put Request.
        /// </summary>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        Task<RestResponse<T>> PutAsync<T>(RestRequest request) where T : new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Delete Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Task<RestResponse> DeleteAsync(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Delete Request.
        /// </summary>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        Task<RestResponse<T>> DeleteAsync<T>(RestRequest request) where T : new();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Head Request.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse"/> instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        Task<RestResponse> HeadAsync(RestRequest request);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes a Head Request.
        /// </summary>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="request">
        ///     The request to use.
        /// </param>
        ///
        /// <returns>
        ///     An <see cref="RestResponse{T}"/> instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        Task<RestResponse<T>> HeadAsync<T>(RestRequest request) where T : new();
    }
}
