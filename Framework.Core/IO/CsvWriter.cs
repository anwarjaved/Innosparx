namespace Framework.IO
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;

    public class CsvWriter : DisposableObject
    {
        private readonly TextWriter writer;

        private bool includeHeader = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CsvWriter" /> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public CsvWriter(TextWriter writer)
        {
            this.QuoteAll = true;
            this.FieldSeparator = ',';
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            this.writer = writer;
        }

        /// <summary>
        ///     Gets or sets the separator used to format date.
        /// </summary>
        /// <value>The separator used to format date.</value>
        public string DateFormat { get; set; }

        /// <summary>
        ///     Gets or sets the separator used to delimit fields.
        /// </summary>
        /// <value>The separator used to delimit fields.</value>
        public char FieldSeparator { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether include header in CSV file.
        /// </summary>
        /// <value><c>true</c> if include header; otherwise, <c>false</c>.</value>
        public bool IncludeHeader
        {
            get
            {
                return this.includeHeader;
            }
            set
            {
                this.includeHeader = value;
            }
        }

        /// <summary>
        ///     Gets or sets the separator used to format number.
        /// </summary>
        /// <value>The separator used to format number.</value>
        public string NumberFormat { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether try to parse number from string useful if number are stored as string data
        ///     type.
        /// </summary>
        /// <value><c>true</c> if try to parse number from string; otherwise, <c>false</c>.</value>
        public bool ParseNumber { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether quote all fields.
        /// </summary>
        /// <value><c>true</c> if quote all fields; otherwise, <c>false</c>.</value>
        public bool QuoteAll { get; set; }

        /// <summary>
        ///     Gets the writer.
        /// </summary>
        /// <value>The writer.</value>
        public TextWriter Writer
        {
            get
            {
                return this.writer;
            }
        }

        /// <summary>
        ///     Closes the Writer Stream and Release resources
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        ///     Writes the specified table to stream.
        /// </summary>
        /// <param name="table">The table to read.</param>
        public virtual void Write(DataTable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            int count = table.Columns.Count;
            if (this.IncludeHeader)
            {
                for (int i = 0; i < count; i++)
                {
                    this.WriteItem(table.Columns[i].Caption);
                    if (i < count - 1)
                    {
                        this.Writer.Write(this.FieldSeparator);
                    }
                    else
                    {
                        this.Writer.Write(Environment.NewLine);
                    }
                }
            }
            foreach (DataRow row in table.Rows)
            {
                for (int index = 0; index < count; index++)
                {
                    Type parentDataType = table.Columns[index].DataType;
                    this.WriteItem(row, index, parentDataType);
                    if (index < count - 1)
                    {
                        this.Writer.Write(this.FieldSeparator);
                    }
                    else
                    {
                        this.Writer.Write(Environment.NewLine);
                    }
                }
            }
        }

        /// <summary>
        ///     Writes the specified Relation to stream.
        /// </summary>
        /// <param name="relation">The relation.</param>
        public virtual void Write(DataRelation relation)
        {
            if (relation == null)
            {
                throw new ArgumentNullException("relation");
            }
            DataTable parentTable = relation.ParentTable;
            DataTable childTable = relation.ChildTable;
            if (parentTable == null)
            {
                throw new ArgumentException("relation");
            }
            if (childTable == null)
            {
                throw new ArgumentException("relation");
            }
            int count = parentTable.Columns.Count;
            int count2 = childTable.Columns.Count;
            if (this.IncludeHeader)
            {
                for (int i = 0; i < count; i++)
                {
                    this.WriteItem(parentTable.Columns[i].Caption);
                    if (i < count - 1)
                    {
                        this.Writer.Write(this.FieldSeparator);
                    }
                }
                this.Writer.Write(this.FieldSeparator);
                for (int i = 0; i < count2; i++)
                {
                    this.WriteItem(childTable.Columns[i].Caption);
                    if (i < count2 - 1)
                    {
                        this.Writer.Write(this.FieldSeparator);
                    }
                    else
                    {
                        this.Writer.Write(Environment.NewLine);
                    }
                }
            }
            foreach (DataRow row in parentTable.Rows)
            {
                DataRow[] childRows = row.GetChildRows(relation);

                if (childRows != null && childRows.Length > 0)
                {
                    foreach (DataRow childRow in childRows)
                    {
                        for (int index2 = 0; index2 < count; index2++)
                        {
                            Type parentDataType = parentTable.Columns[index2].DataType;
                            this.WriteItem(row, index2, parentDataType);
                            this.Writer.Write(this.FieldSeparator);
                        }
                        for (int index = 0; index < count2; index++)
                        {
                            Type childDataType = childTable.Columns[index].DataType;
                            this.WriteItem(childRow, index, childDataType);
                            if (index < count2 - 1)
                            {
                                this.Writer.Write(this.FieldSeparator);
                            }
                            else
                            {
                                this.Writer.Write(Environment.NewLine);
                            }
                        }
                    }
                }
                else
                {
                    for (int index = 0; index < count; index++)
                    {
                        Type parentDataType = parentTable.Columns[index].DataType;
                        this.WriteItem(row, index, parentDataType);
                        if (index < count2 - 1)
                        {
                            this.Writer.Write(this.FieldSeparator);
                        }
                        else
                        {
                            this.Writer.Write(Environment.NewLine);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Override This Method To Dispose Managed Resources.
        /// </summary>
        protected override void DisposeResources()
        {
            this.writer.Dispose();
            base.DisposeResources();
        }

        /// <summary>
        ///     Writes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        protected virtual void WriteItem(object item)
        {
            if (item == null)
            {
                return;
            }
            string s = item.ToString(); //.Replace("\r", " ").Replace("\n", " ");
            if (!string.IsNullOrEmpty(s))
            {
                if (this.QuoteAll || s.IndexOfAny("\",\x0A\x0D".ToCharArray()) > -1)
                {
                    s = s.Replace("\"", "\"\"");
                    this.Writer.Write("\"" + s + "\"");
                }
                else
                {
                    this.Writer.Write(s);
                }
            }
        }

        private static bool IsNumeric(string expression)
        {
            double retNum;
            bool isNum = Double.TryParse(expression, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        private void WriteItem(DataRow row, int index, Type dataType)
        {
            if (!string.IsNullOrEmpty(this.DateFormat) && dataType == typeof(DateTime))
            {
                object item = row[index];
                if (item != null)
                {
                    string s = string.Format(this.DateFormat, item);
                    if (!string.IsNullOrEmpty(s))
                    {
                        this.WriteItem(s);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(this.NumberFormat)
                     && (dataType == typeof(decimal) || dataType == typeof(double) || dataType == typeof(short)
                         || dataType == typeof(int) || dataType == typeof(long) || dataType == typeof(float)
                         || dataType == typeof(ushort) || dataType == typeof(uint) || dataType == typeof(ulong)))
            {
                object item = row[index];
                if (item != null)
                {
                    string s = string.Format(this.NumberFormat, item);
                    if (!string.IsNullOrEmpty(s))
                    {
                        this.WriteItem(s);
                    }
                }
            }
            else
            {
                object item = row[index];
                //try
                if (item != null)
                {
                    if (this.ParseNumber && !string.IsNullOrEmpty(this.NumberFormat) && IsNumeric(item.ToString())) // evaluates to true
                    {
                        string s = string.Format(this.NumberFormat, Convert.ToDecimal(item));
                        if (!string.IsNullOrEmpty(s))
                        {
                            this.WriteItem(s);
                        }
                    }
                    else
                    {
                        this.WriteItem(item);
                    }
                }
            }
        }
    }
}