namespace Framework.IO
{
    using System;
    using System.Data;
    using System.IO;

    /// <summary>
    ///     A class used to read CSV Files.
    /// </summary>
    public class CsvReader : DisposableObject
    {
        private readonly TextReader reader;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CsvReader" /> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public CsvReader(TextReader reader)
        {
            this.IncludeHeader = true;
            if (reader == null)
            {
                throw new ArgumentNullException("writer");
            }
            this.reader = reader;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether include header in CSV file.
        /// </summary>
        /// <value><c>true</c> if include header; otherwise, <c>false</c>.</value>
        public bool IncludeHeader { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this reader is closed.
        /// </summary>
        /// <value><c>true</c> if this reader is closed; otherwise, <c>false</c>.</value>
        public bool IsClosed
        {
            get
            {
                return (this.reader.Peek() == -1);
            }
        }

        /// <summary>
        ///     Gets the reader.
        /// </summary>
        /// <value>The reader.</value>
        public TextReader Reader
        {
            get
            {
                return this.reader;
            }
        }

        /// <summary>
        ///     Closes the Reader Stream and Release resources
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        ///     Parses the reader stream and return results in a <see cref="DataTable" />.
        /// </summary>
        /// <returns></returns>
        public DataTable Parse()
        {
            if (this.IsClosed)
            {
                throw new InvalidProgramException("stream is closed.");
            }

            var table = new DataTable();
            var csv = new CsvStream(this.Reader);
            string[] row = csv.GetNextRow();
            if (row == null)
            {
                return null;
            }
            if (this.IncludeHeader)
            {
                foreach (string header in row)
                {
                    if (!string.IsNullOrEmpty(header) && !table.Columns.Contains(header))
                    {
                        table.Columns.Add(header, typeof(string));
                    }
                    else
                    {
                        table.Columns.Add(GetNextColumnHeader(table), typeof(string));
                    }
                }
                row = csv.GetNextRow();
            }
            while (row != null)
            {
                while (row.Length > table.Columns.Count)
                {
                    table.Columns.Add(GetNextColumnHeader(table), typeof(string));
                }
                table.Rows.Add(row);
                row = csv.GetNextRow();
            }
            return table;
        }

        /// <summary>
        ///     Override This Method To Dispose Managed Resources.
        /// </summary>
        protected override void DisposeResources()
        {
            this.reader.Dispose();
            base.DisposeResources();
        }

        private static string GetNextColumnHeader(DataTable table)
        {
            int c = 1;
            while (true)
            {
                string h = "Column" + c++;
                if (!table.Columns.Contains(h))
                {
                    return h;
                }
            }
        }
    }
}