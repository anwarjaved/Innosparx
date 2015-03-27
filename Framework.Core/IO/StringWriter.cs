namespace Framework.IO
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>
    ///     Implements a TextWriter for writing information to a string.
    ///     The information is stored in an underlying StringBuilder.
    /// </summary>
    public class StringWriter : System.IO.StringWriter
    {
        private readonly Encoding encoding;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringWriter" /> class.
        /// </summary>
        public StringWriter()
            : base(CultureInfo.CurrentCulture)
        {
        } // StringWriter

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringWriter" /> class.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        public StringWriter(Encoding encoding)
            : base(CultureInfo.CurrentCulture)
        {
            this.encoding = encoding;
        } // StringWriter

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringWriter" /> class.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="formatProvider">An <see cref="System.IFormatProvider"></see> object that controls formatting.</param>
        public StringWriter(Encoding encoding, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            this.encoding = encoding;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringWriter" /> class.
        /// </summary>
        /// <param name="formatProvider">An <see cref="System.IFormatProvider"></see> object that controls formatting.</param>
        public StringWriter(IFormatProvider formatProvider)
            : base(formatProvider)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringWriter" /> class.
        /// </summary>
        /// <param name="sb">The sb.</param>
        public StringWriter(StringBuilder sb)
            : base(sb, CultureInfo.CurrentCulture)
        {
        } // StringWriter

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringWriter" /> class.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="sb">The sb.</param>
        public StringWriter(Encoding encoding, StringBuilder sb)
            : base(sb, CultureInfo.CurrentCulture)
        {
            this.encoding = encoding;
        } // StringWriter

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringWriter" /> class.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="formatProvider">The format provider.</param>
        public StringWriter(StringBuilder sb, IFormatProvider formatProvider)
            : base(sb, formatProvider)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringWriter" /> class.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="encoding">The encoding.</param>
        public StringWriter(StringBuilder sb, IFormatProvider formatProvider, Encoding encoding)
            : base(sb, formatProvider)
        {
            this.encoding = encoding;
        }

        /// <summary>
        ///     Gets the <see cref="System.Text.Encoding"></see> in which the output is written.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///     The <see cref="System.Text.Encoding"></see> specified in the constructor for the current instance, or
        ///     <see cref="System.Text.UTF8Encoding"></see> if an encoding was not specified.
        /// </returns>
        public override Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
        }
    }
}