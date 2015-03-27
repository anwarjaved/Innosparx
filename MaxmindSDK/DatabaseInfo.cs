namespace MaxmindSDK
{
    using System;

    public class DatabaseInfo
    {
        private readonly string info;

        public DatabaseInfo(string info)
        {
            this.info = info;
        }

        public DatabaseType Type
        {
            get
            {
                if (string.IsNullOrEmpty(this.info))
                {
                    return DatabaseType.CountryEdition;
                }

                // Get the type code from the database info string and then
                // subtract 105 from the value to preserve compatability with
                // databases from April 2003 and earlier.
                return (DatabaseType)(Convert.ToInt32(this.info.Substring(4, 3)) - 105);
            }
        }

        public bool IsPremium
        {
            get
            {
                return this.info.IndexOf("FREE", StringComparison.Ordinal) < 0;
            }
        }

        /// <summary>
        /// Gets the date of the database.
        /// </summary>
        public DateTime Date
        {
            get
            {
                for (int i = 0; i < this.info.Length - 9; i++)
                {
                    if (char.IsWhiteSpace(this.info[i]))
                    {
                        String dateString = this.info.Substring(i + 1, 8);
                        return DateTime.ParseExact(dateString, "yyyyMMdd", null);
                    }
                }

                return DateTime.Now;
            }
        }

        public override string ToString()
        {
            return this.info;
        }
    }
}
