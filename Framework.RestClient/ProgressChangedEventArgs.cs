using System;

namespace Framework.Rest
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Provides data for the UploadProgressChanged event of a RestClient.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class ProgressChangedEventArgs : EventArgs
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the bytes received.
        /// </summary>
        ///
        /// <value>
        ///     The bytes received.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public long BytesReceived { get; internal set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the bytes sent.
        /// </summary>
        ///
        /// <value>
        ///     The bytes sent.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public long BytesSent { get; internal set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the total bytes to receive.
        /// </summary>
        ///
        /// <value>
        ///     The total number of bytes to receive.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public long TotalBytesToReceive { get; internal set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the total bytes to send.
        /// </summary>
        ///
        /// <value>
        ///     The total number of bytes to send.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public long TotalBytesToSend { get; internal set; }
    }
}
