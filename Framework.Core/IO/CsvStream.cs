namespace Framework.IO
{
    using System.Collections;
    using System.IO;
    using System.Text;

    internal class CsvStream
    {
        private readonly char[] buffer = new char[4096];

        private readonly TextReader stream;

        private bool endOfLine;

        private bool endOfString;

        private int length;

        private int pos;

        public CsvStream(TextReader s)
        {
            this.stream = s;
        }

        public string[] GetNextRow()
        {
            var row = new ArrayList();
            while (true)
            {
                string item = this.GetNext();
                if (item == null)
                {
                    return row.Count == 0 ? null : (string[])row.ToArray(typeof(string));
                }
                row.Add(item);
            }
        }

        private string GetNext()
        {
            if (this.endOfLine)
            {
                // previous item was last in line, start new line
                this.endOfLine = false;
                return null;
            }

            bool quoted = false;
            bool predata = true;
            bool postdata = false;
            var item = new StringBuilder();

            while (true)
            {
                char c = this.GetNextChar(true);
                if (this.endOfString)
                {
                    return item.Length > 0 ? item.ToString() : null;
                }

                if ((postdata || !quoted) && c == ',') // end of item, return
                {
                    return item.ToString();
                }

                if ((predata || postdata || !quoted) && (c == '\x0A' || c == '\x0D'))
                {
                    // we are at the end of the line, eat newline characters and exit
                    this.endOfLine = true;
                    if (c == '\x0D' && this.GetNextChar(false) == '\x0A') // new line sequence is 0D0A
                    {
                        this.GetNextChar(true);
                    }
                    return item.ToString();
                }

                if (predata && c == ' ') // whitespace preceeding data, discard
                {
                    continue;
                }

                if (predata && c == '"')
                {
                    // quoted data is starting
                    quoted = true;
                    predata = false;
                    continue;
                }

                if (predata)
                {
                    // data is starting without quotes
                    predata = false;
                    item.Append(c);
                    continue;
                }

                if (c == '"' && quoted)
                {
                    if (this.GetNextChar(false) == '"') // double quotes within quoted string means add a quote       
                    {
                        item.Append(this.GetNextChar(true));
                    }
                    else
                    {
                        // end-quote reached
                        postdata = true;
                    }
                    continue;
                }

                // all cases covered, character must be data
                item.Append(c);
            }
        }

        private char GetNextChar(bool eat)
        {
            if (this.pos >= this.length)
            {
                this.length = this.stream.ReadBlock(this.buffer, 0, this.buffer.Length);
                if (this.length == 0)
                {
                    this.endOfString = true;
                    return '\0';
                }
                this.pos = 0;
            }
            return eat ? this.buffer[this.pos++] : this.buffer[this.pos];
        }
    }
}