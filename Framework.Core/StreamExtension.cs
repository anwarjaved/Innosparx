namespace Framework
{
    using System.ComponentModel;
    using System.IO;

    /// <summary>
    /// Extention Methods For <see cref="Stream "/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StreamExtension
    {
        /// <summary>
        /// Writes the entire contents of this stream to another stream using a buffer
        /// with the specified size.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="stream">The stream to write this stream to.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public static void WriteTo(this Stream sourceStream, Stream stream, int bufferSize = FrameworkConstants.BufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int n;
            while ((n = sourceStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                stream.Write(buffer, 0, n);
            }
        }

        /// <summary>
        /// Writes the entire contents of this stream to byte array.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <returns>Byte Array contaiing Stream content.</returns>
        public static byte[] ToArray(this Stream sourceStream)
        {
            byte[] buffer = new byte[FrameworkConstants.BufferSize];
            using (MemoryStream ms = new MemoryStream())
            {
                int count;
                while ((count = sourceStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    ms.Write(buffer, 0, count);
                }

                return ms.ToArray();
            }
        }
    }
}