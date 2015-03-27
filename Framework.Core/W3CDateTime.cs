namespace Framework
{
    using System;

    using System.Globalization;

    using System.Text.RegularExpressions;

    /// <summary>
    /// Representation of dates and times is W3C Format (ISO 8601 or RFC822).
    /// </summary>
    public struct W3CDateTime : IComparable<W3CDateTime>, IEquatable<W3CDateTime>,
                              IComparable
    {
        private const string Rfc822DateFormat =
          @"^((Mon|Tue|Wed|Thu|Fri|Sat|Sun), *)?(?<day>\d\d?) +" +
          @"(?<month>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) +" +
          @"(?<year>\d\d(\d\d)?) +" + @"(?<hour>\d\d):(?<min>\d\d)(:(?<sec>\d\d))? +" +
          @"(?<ofs>([+\-]?\d\d\d\d)|UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT)$";

        private const string W3CDateFormat =
          @"^(?<year>\d\d\d\d)" +
          @"(-(?<month>\d\d)(-(?<day>\d\d)(T(?<hour>\d\d):

(?<min>\d\d)(:(?<sec>\d\d)(?<ms>\.\d+)?)?" +
          @"(?<ofs>(Z|[+\-]\d\d:\d\d)))?)?)?$";

        private static readonly string CombinedFormat = @"(?<rfc822>{0})|(?<w3c>{1})".FormatString(Rfc822DateFormat, W3CDateFormat);

        private static readonly Regex W3CDateRegex = new Regex(CombinedFormat, RegexOptions.Compiled);

        private static readonly string[] MonthNames =
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep",
            "Oct", "Nov", "Dec"
        };

        private DateTime localDate;
        private TimeSpan offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="W3CDateTime"/> struct from the passed <c>datetime</c> and offset.
        /// </summary>
        /// <param name="localDate">The time in the local time zone.</param>
        /// <param name="offset">Offset from <paramref name="localDate"/> to universal time.</param>
        public W3CDateTime(DateTime localDate, TimeSpan offset)
        {
            this.localDate = localDate;
            this.offset = offset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="W3CDateTime"/> struct from the passed <see cref="DateTime"/>.
        /// Offset is assumed to be the local offset.
        /// </summary>
        /// <param name="dt">The time in the local time zone.</param>
        public W3CDateTime(DateTime dt)
            : this(dt, LocalUtcOffset)
        {
        }

        /// <summary>
        /// Gets the local UTC offset.
        /// </summary>
        /// <value>The local UTC offset.</value>
        public static TimeSpan LocalUtcOffset
        {
            get
            {
                return TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
            }
        }

        /// <summary>
        /// Gets the largest possible value of <see cref="W3CDateTime"/>.
        /// </summary>
        /// <value>The max value.</value>
        public static W3CDateTime MaxValue
        {
            get { return new W3CDateTime(DateTime.MaxValue, TimeSpan.Zero); }
        }

        /// <summary>
        /// Gets the smallest possible value of <see cref="W3CDateTime"/>.
        /// </summary>
        /// <value>The min value.</value>
        public static W3CDateTime MinValue
        {
            get { return new W3CDateTime(DateTime.MinValue, TimeSpan.Zero); }
        }

        /// <summary>Gets a <see cref="W3CDateTime"/> object that is set to the current date and time on this computer, expressed as the local time.</summary>
        /// <value>A <see cref="W3CDateTime"/> whose value is the current local date and time.</value>
        public static W3CDateTime Now
        {
            get { return new W3CDateTime(DateTime.Now); }
        }

        /// <summary>
        /// Gets the current date.
        /// </summary>
        /// <value>The today.</value>
        public static W3CDateTime Today
        {
            get { return new W3CDateTime(DateTime.Today); }
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> object that is set to the current date and time on this computer,
        /// expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The UTC now.</value>
        public static W3CDateTime UtcNow
        {
            get { return new W3CDateTime(DateTime.UtcNow, TimeSpan.Zero); }
        }

        /// <summary>
        /// Gets the local date time.
        /// </summary>
        /// <value>The local date time.</value>
        public DateTime LocalDateTime
        {
            get { return this.localDate; }
        }

        /// <summary>
        /// Gets the local time.
        /// </summary>
        /// <value>The local time.</value>
        public DateTime LocalTime
        {
            get { return this.UtcTime + LocalUtcOffset; }
        }

        /// <summary>
        /// Gets a <see cref="TimeSpan"/> object that offset from localDate to universal time.
        /// </summary>
        /// <value>The UTC offset.</value>
        public TimeSpan UtcOffset
        {
            get { return this.offset; }
        }

        /// <summary>
        /// Gets the UTC time.
        /// </summary>
        /// <value>The UTC time.</value>
        public DateTime UtcTime
        {
            get { return this.localDate - this.offset; }
        }

        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="DateTime"/> are equal.
        /// </summary>
        /// <param name="t1">The first <see cref="DateTime"/> instance.</param>
        /// <param name="t2">The second <see cref="DateTime"/> instance.</param>
        /// <returns>true if the two <see cref="DateTime"/> values are equal; otherwise, false. </returns>
        public static bool Equals(W3CDateTime t1, W3CDateTime t2)
        {
            return DateTime.Equals(t1.UtcTime, t2.UtcTime);
        }

        /// <summary>Converts the specified string representation of a date and time to its <see cref="W3CDateTime"/> equivalent.</summary>
        /// <returns>A <see cref="W3CDateTime"/> equivalent to the date and time contained in s.</returns>
        /// <param name="text">A string containing a date and time to convert. </param>
        /// <exception cref="T:System.ArgumentNullException">s is null. </exception>
        /// <exception cref="FormatException">s does not contain a valid string representation of a date and time. </exception>
        public static W3CDateTime Parse(string text)
        {
            // try to parse it
            Match m = W3CDateRegex.Match(text);
            if (!m.Success)
            {
                // Didn't match either expression. Throw an exception.
                throw new FormatException("String is not a valid date time stamp.");
            }

            try
            {
                bool success = m.Groups["rfc822"].Success;
                int year = int.Parse(m.Groups["year"].Value, CultureInfo.CurrentCulture);

                // handle 2-digit and 3-digit years
                if (year < 1000)
                {
                    if (year < 50)
                    {
                        year = year + 2000;
                    }
                    else
                    {
                        year = year + 1999;
                    }
                }

                int month;
                if (success)
                {
                    month = ParseRfc822Month(m.Groups["month"].Value);
                }
                else
                {
                    month = m.Groups["month"].Success
                              ? int.Parse(m.Groups["month"].Value, CultureInfo.CurrentCulture)
                              : 1;
                }

                int day = m.Groups["day"].Success
                            ? int.Parse(m.Groups["day"].Value, CultureInfo.CurrentCulture)
                            : 1;
                int hour = m.Groups["hour"].Success
                            ? int.Parse(m.Groups["hour"].Value, CultureInfo.CurrentCulture)
                            : 0;
                int min = m.Groups["min"].Success
                            ? int.Parse(m.Groups["min"].Value, CultureInfo.CurrentCulture)
                            : 0;
                int sec = m.Groups["sec"].Success
                            ? int.Parse(m.Groups["sec"].Value, CultureInfo.CurrentCulture)
                            : 0;
                int ms = m.Groups["ms"].Success
                          ? (int)
                            Math.Round(1000 * double.Parse(m.Groups["ms"].Value, CultureInfo.CurrentCulture)) : 0;

                TimeSpan ofs = TimeSpan.Zero;

                if (m.Groups["ofs"].Success)
                {
                    ofs = success
                            ? ParseRfc822Offset(m.Groups["ofs"].Value)
                            : ParseW3COffset(m.Groups["ofs"].Value);
                }

                return new W3CDateTime(new DateTime(year, month, day, hour, min, sec, ms), ofs);
            }
            catch (Exception ex)
            {
                throw new FormatException("String is not a valid date time stamp.", ex);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts the specified string representation of a date and time to its<see cref="W3CDateTime"/>equivalent.
        /// </summary>
        /// <param name="text">
        /// A string containing a date and time to convert.
        /// </param>
        /// <param name="date">
        /// A<see cref="W3CDateTime"/>.
        /// </param>
        /// <returns>
        /// A<see cref="W3CDateTime"/>equivalent to the date and time contained in s.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static bool TryParse(string text, out W3CDateTime date)
        {
            date = MinValue;

            // try to parse it
            Match m = W3CDateRegex.Match(text);
            if (m.Success)
            {
                try
                {
                    bool success = m.Groups["rfc822"].Success;

                    int year = int.Parse(m.Groups["year"].Value, CultureInfo.CurrentCulture);

                    // handle 2-digit and 3-digit years
                    if (year < 1000)
                    {
                        if (year < 50)
                        {
                            year = year + 2000;
                        }
                        else
                        {
                            year = year + 1999;
                        }
                    }

                    int month;
                    if (success)
                    {
                        month = ParseRfc822Month(m.Groups["month"].Value);
                    }
                    else
                    {
                        month = m.Groups["month"].Success
                                    ? int.Parse(m.Groups["month"].Value, CultureInfo.CurrentCulture)
                                    : 1;
                    }

                    int day = m.Groups["day"].Success ? int.Parse(m.Groups["day"].Value, CultureInfo.CurrentCulture) : 1;
                    int hour = m.Groups["hour"].Success
                                   ? int.Parse(m.Groups["hour"].Value, CultureInfo.CurrentCulture)
                                   : 0;
                    int min = m.Groups["min"].Success ? int.Parse(m.Groups["min"].Value, CultureInfo.CurrentCulture) : 0;
                    int sec = m.Groups["sec"].Success ? int.Parse(m.Groups["sec"].Value, CultureInfo.CurrentCulture) : 0;
                    int ms = m.Groups["ms"].Success
                                 ? (int)
                                   Math.Round(1000 * double.Parse(m.Groups["ms"].Value, CultureInfo.CurrentCulture))
                                 : 0;

                    TimeSpan ofs = TimeSpan.Zero;

                    if (m.Groups["ofs"].Success)
                    {
                        ofs = success
                                  ? ParseRfc822Offset(m.Groups["ofs"].Value)
                                  : ParseW3COffset(m.Groups["ofs"].Value);
                    }

                    date = new W3CDateTime(new DateTime(year, month, day, hour, min, sec, ms), ofs);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }
        
        /// <summary>Adds a specified time interval to a specified date and time, yielding a new date and time.</summary>
        /// <returns>A <see cref="W3CDateTime"/> that is the sum of the values of d and t.</returns>
        /// <param name="date">A <see cref="W3CDateTime"/>. </param>
        /// <param name="time">A <see cref="T:System.TimeSpan"></see>. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="W3CDateTime"/> is less than <see cref="F:System.DateTime.MinValue"></see> or greater than <see cref="F:System.DateTime.MaxValue"></see>. </exception>
        public static W3CDateTime operator +(W3CDateTime date, TimeSpan time)
        {
            return date.Add(time);
        }

        /// <summary>Determines whether two specified instances of <see cref="W3CDateTime"/> are equal.</summary>
        /// <returns>true if d1 and d2 represent the same date and time; otherwise, false.</returns>
        /// <param name="d1">First <see cref="W3CDateTime"/>. </param>
        /// <param name="d2">Second <see cref="W3CDateTime"/>. </param>
        public static bool operator ==(W3CDateTime d1, W3CDateTime d2)
        {
            return Equals(d1, d2);
        }

        /// <summary>Determines whether one specified <see cref="W3CDateTime"/> is greater than another specified <see cref="W3CDateTime"/>.</summary>
        /// <returns>True if t1 is greater than t2; otherwise, false.</returns>
        /// <param name="t1">First <see cref="W3CDateTime"/>. </param>
        /// <param name="t2">Second <see cref="W3CDateTime"/>. </param>
        public static bool operator >(W3CDateTime t1, W3CDateTime t2)
        {
            return Compare(t1, t2) > 0;
        }

        /// <summary>Determines whether one specified <see cref="W3CDateTime"/> is greater than or equal to another specified <see cref="W3CDateTime"/>.</summary>
        /// <returns>true if t1 is greater than or equal to t2; otherwise, false.</returns>
        /// <param name="t1">First <see cref="W3CDateTime"/>. </param>
        /// <param name="t2">Second <see cref="W3CDateTime"/>. </param>
        public static bool operator >=(W3CDateTime t1, W3CDateTime t2)
        {
            return Compare(t1, t2) >= 0;
        }

        /// <summary>Determines whether two specified instances of <see cref="W3CDateTime"/> are not equal.</summary>
        /// <returns>true if d1 and d2 do not represent the same date and time; otherwise, false.</returns>
        /// <param name="d1">First <see cref="W3CDateTime"/>. </param>
        /// <param name="d2">Second <see cref="W3CDateTime"/>. </param>
        public static bool operator !=(W3CDateTime d1, W3CDateTime d2)
        {
            return !Equals(d1, d2);
        }

        /// <summary>
        /// Implicit Converts <see cref="DateTime"/> to <see cref="W3CDateTime"/>.
        /// </summary>
        /// <param name="value"><see cref="DateTime"/> To Convert.</param>
        /// <returns><see cref="DateTime"/> as W3CDateTime.</returns>
        public static implicit operator W3CDateTime(DateTime value)
        {
            return new W3CDateTime(value);
        }

        /// <summary>
        /// Implicit Converts <see cref="W3CDateTime"/> to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value"><see cref="W3CDateTime"/> To Convert.</param>
        /// <returns><see cref="W3CDateTime"/> as <see cref="DateTime"/>.</returns>
        public static implicit operator DateTime(W3CDateTime value)
        {
            return value.LocalDateTime;
        }

        /// <summary>Determines whether one specified <see cref="W3CDateTime"/> is less than another specified <see cref="W3CDateTime"/>.</summary>
        /// <returns>true if t1 is less than t2; otherwise, false.</returns>
        /// <param name="t1">First <see cref="W3CDateTime"/>. </param>
        /// <param name="t2">Second <see cref="W3CDateTime"/>. </param>
        public static bool operator <(W3CDateTime t1, W3CDateTime t2)
        {
            return Compare(t1, t2) < 0;
        }

        /// <summary>Determines whether one specified <see cref="W3CDateTime"/> is less than or equal to another specified <see cref="W3CDateTime"/>.</summary>
        /// <returns>true if t1 is less than or equal to t2; otherwise, false.</returns>
        /// <param name="t1">First <see cref="W3CDateTime"/>. </param>
        /// <param name="t2">Second <see cref="W3CDateTime"/>. </param>
        public static bool operator <=(W3CDateTime t1, W3CDateTime t2)
        {
            return Compare(t1, t2) <= 0;
        }

        /// <summary>Subtracts a specified date and time from another specified date and time, yielding a time interval.</summary>
        /// <returns>A <see cref="T:System.TimeSpan"></see> that is the time interval between d1 and d2; that is, d1 minus d2.</returns>
        /// <param name="d1">A <see cref="W3CDateTime"/> (the minuend). </param>
        /// <param name="d2">A <see cref="W3CDateTime"/> (the subtrahend). </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="W3CDateTime"/> is less than <see cref="F:System.DateTime.MinValue"></see> or greater than <see cref="F:System.DateTime.MaxValue"></see>. </exception>
        public static TimeSpan operator -(W3CDateTime d1, W3CDateTime d2)
        {
            return d1.Subtract(d2);
        }

        /// <summary>Subtracts a specified time interval from a specified date and time, yielding a new date and time.</summary>
        /// <returns>A <see cref="W3CDateTime"/> whose value is the value of d minus the value of t.</returns>
        /// <param name="date">A <see cref="W3CDateTime"/>. </param>
        /// <param name="time">A <see cref="T:System.TimeSpan"></see>. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="W3CDateTime"/> is less than <see cref="F:System.DateTime.MinValue"></see> or greater than <see cref="F:System.DateTime.MaxValue"></see>. </exception>
        public static W3CDateTime operator -(W3CDateTime date, TimeSpan time)
        {
            return date.Subtract(time);
        }

        /// <summary>Compares two instances of <see cref="W3CDateTime"/> and returns an indication of their relative values.</summary>
        /// <returns>A signed number indicating the relative values of t1 and t2.Value Type Condition Less than zero t1 is less than t2. Zero t1 equals t2. Greater than zero t1 is greater than t2. </returns>
        /// <param name="t1">The first <see cref="W3CDateTime"/>. </param>
        /// <param name="t2">The second <see cref="W3CDateTime"/>. </param>
        public static int Compare(W3CDateTime t1, W3CDateTime t2)
        {
            return DateTime.Compare(t1.UtcTime, t2.UtcTime);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts this object to a date.
        /// </summary>
        /// <returns>
        ///     This object as a W3CDateTime.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public W3CDateTime ToDate()
        {
            return new W3CDateTime(new DateTime(this.localDate.Year, this.localDate.Month, this.localDate.Day), this.UtcOffset);
        }

        /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
        /// <returns>A signed number indicating the relative values of this instance and value.Value Description Less than zero This instance is less than value. Zero This instance is equal to value. Greater than zero This instance is greater than value, or value is null. </returns>
        /// <param name="value">A boxed <see cref="W3CDateTime"/> object to compare, or null. </param>
        /// <exception cref="T:System.ArgumentException">value is not a <see cref="W3CDateTime"/>. </exception>
        public int CompareTo(object value)
        {
            return this.CompareTo((W3CDateTime)value);
        }

        /// <summary>Compares this instance to a specified <see cref="W3CDateTime"/> object and returns an indication of their relative values.</summary>
        /// <returns>A signed number indicating the relative values of this instance and the value parameter.Value Description Less than zero This instance is less than value. Zero This instance is equal to value. Greater than zero This instance is greater than value. </returns>
        /// <param name="other">A <see cref="W3CDateTime"/> object to compare. </param>
        public int CompareTo(W3CDateTime other)
        {
            return Compare(this, other);
        }

        /// <summary>Returns a value indicating whether this instance is equal to the specified <see cref="W3CDateTime"/> instance.</summary>
        /// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
        /// <param name="other">A <see cref="W3CDateTime"/> instance to compare to this instance. </param>
        public bool Equals(W3CDateTime other)
        {
            return DateTime.Equals(this.UtcTime, other.UtcTime);
        }

        /// <summary>
        /// Adds the value of the specified <see cref="TimeSpan"/> to the value of this instance. 
        /// </summary>
        /// <param name="value">A <see cref="TimeSpan"/> that contains the interval to add.</param>
        /// <returns><see cref="W3CDateTime"/> value is the sum of the date and time represented by 
        /// this instance and the time interval represented by value.</returns>
        public W3CDateTime Add(TimeSpan value)
        {
            return new W3CDateTime(this.localDate + value, this.offset);
        }

        /// <summary>
        /// Adds the specified number of days to the value of this instance.
        /// </summary>
        /// <param name="value">A number of whole and fractional days. 
        /// The value parameter can be negative or positive.</param>
        /// <returns><see cref="W3CDateTime"/> value is the sum of the date and time 
        /// represented by this instance and the number of days represented by value. </returns>
        public W3CDateTime AddDays(double value)
        {
            return new W3CDateTime(this.localDate.AddDays(value), this.offset);
        }

        /// <summary>
        /// Adds the specified number of hours to the value of this instance. 
        /// </summary>
        /// <param name="value">A number of whole and fractional hours. The value 
        /// parameter can be negative or positive. </param>
        /// <returns><see cref="W3CDateTime"/> value is the sum of the date and time represented 
        /// by this instance and the number of hours represented by value. </returns>
        public W3CDateTime AddHours(double value)
        {
            return new W3CDateTime(this.localDate.AddHours(value), this.offset);
        }

        /// <summary>
        /// Adds the specified number of milliseconds to the value of this instance. 
        /// </summary>
        /// <param name="value">A number of whole and fractional milliseconds. 
        /// The value parameter can be negative or positive. 
        /// this value is rounded to the nearest integer.</param>
        /// <returns><see cref="W3CDateTime"/> value is the sum of the date and time represented 
        /// by this instance and the number of milliseconds represented by value.</returns>
        public W3CDateTime AddMilliseconds(double value)
        {
            return new W3CDateTime(this.localDate.AddMilliseconds(value), this.offset);
        }

        /// <summary>
        /// Adds the specified number of minutes to the value of this instance. 
        /// </summary>
        /// <param name="value">A number of whole and fractional minutes. The value parameter can be negative or positive.</param>
        /// <returns><see cref="W3CDateTime"/> value is the sum of the date and time represented by this instance and the number of minutes represented by value.</returns>
        public W3CDateTime AddMinutes(double value)
        {
            return new W3CDateTime(this.localDate.AddMinutes(value), this.offset);
        }

        /// <summary>
        /// Adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="value">A number of months. The months parameter can be negative or positive.</param>
        /// <returns><see cref="W3CDateTime"/> value is the sum of the date and time represented by this instance and months.</returns>
        public W3CDateTime AddMonths(int value)
        {
            return new W3CDateTime(this.localDate.AddMonths(value), this.offset);
        }

        /// <summary>
        /// Adds the specified number of seconds to the value of this instance.
        /// </summary>
        /// <param name="value">A number of whole and fractional seconds. The value parameter can be negative or positive.</param>
        /// <returns><see cref="W3CDateTime"/> value is the sum of the date and time represented by this instance and the number of seconds represented by value.</returns>
        public W3CDateTime AddSeconds(double value)
        {
            return new W3CDateTime(this.localDate.AddSeconds(value), this.offset);
        }

        /// <summary>
        /// Adds the specified number of ticks to the value of this instance.
        /// </summary>
        /// <param name="value">A number of 100-nanosecond ticks. The value parameter can be positive or negative.</param>
        /// <returns><see cref="W3CDateTime"/> value is the sum of the date and time represented by this instance and the time represented by value.</returns>
        public W3CDateTime AddTicks(long value)
        {
            return new W3CDateTime(this.localDate.AddTicks(value), this.offset);
        }

        /// <summary>
        /// Adds the specified number of years to the value of this instance.
        /// </summary>
        /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
        /// <returns><see cref="W3CDateTime"/> value is the sum of the date and time represented by this instance and the number of years represented by value.</returns>
        public W3CDateTime AddYears(int value)
        {
            return new W3CDateTime(this.localDate.AddYears(value), this.offset);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">An object to compare to this instance. </param>
        /// <returns>
        /// True if value is an instance of <see cref="DateTime"/> and equals the value of this instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals((W3CDateTime)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyFieldInGetHashCode
            return this.localDate.GetHashCode() ^ this.offset.GetHashCode();
            // ReSharper restore NonReadonlyFieldInGetHashCode
        }

        /// <summary>Subtracts the specified date and time from this instance.</summary>
        /// <returns>A <see cref="T:System.TimeSpan"></see> interval equal to the date and time represented by this instance minus the date and time represented by value.</returns>
        /// <param name="value">An instance of <see cref="W3CDateTime"/>. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The result is less than <see cref="F:System.DateTime.MinValue"></see> or greater than <see cref="F:System.DateTime.MaxValue"></see>. </exception>
        public TimeSpan Subtract(W3CDateTime value)
        {
            return this.UtcTime.Subtract(value.UtcTime);
        }

        /// <summary>Subtracts the specified duration from this instance.</summary>
        /// <returns>A <see cref="W3CDateTime"/> equal to the date and time represented by this instance minus the time interval represented by value.</returns>
        /// <param name="value">An instance of <see cref="T:System.TimeSpan"></see>. </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The result is less than <see cref="F:System.DateTime.MinValue"></see> or greater than <see cref="F:System.DateTime.MaxValue"></see>. </exception>
        public W3CDateTime Subtract(TimeSpan value)
        {
            return new W3CDateTime(this.localDate.Subtract(value), this.offset);
        }

        /// <summary>
        /// Converts the value of the current <see cref="W3CDateTime"/> object to local time.
        /// </summary>
        /// <param name="localOffset">The local offset.</param>
        /// <returns>
        /// A <see cref="W3CDateTime"/> object whose <see cref="P:System.DateTime.Kind"></see> property is <see cref="F:System.DateTimeKind.Local"></see>, and whose value is the local time equivalent to the value of the current <see cref="W3CDateTime"/> object, or <see cref="F:System.DateTime.MaxValue"></see> if the converted value is too large to be represented by a <see cref="W3CDateTime"/> object, or <see cref="F:System.DateTime.MinValue"></see> if the converted value is too small to be represented as a <see cref="W3CDateTime"/> object.
        /// </returns>
        public W3CDateTime ToLocalTime(TimeSpan localOffset)
        {
            return new W3CDateTime(this.UtcTime - localOffset, localOffset);
        }

        /// <summary>Converts the value of the current <see cref="W3CDateTime"/> object to local time.</summary>
        /// <returns>A <see cref="W3CDateTime"/> object whose <see cref="P:System.DateTime.Kind"></see> property is <see cref="F:System.DateTimeKind.Local"></see>, and whose value is the local time equivalent to the value of the current <see cref="W3CDateTime"/> object, or <see cref="F:System.DateTime.MaxValue"></see> if the converted value is too large to be represented by a <see cref="W3CDateTime"/> object, or <see cref="F:System.DateTime.MinValue"></see> if the converted value is too small to be represented as a <see cref="W3CDateTime"/> object.</returns>
        public W3CDateTime ToLocalTime()
        {
            return this.ToLocalTime(LocalUtcOffset);
        }

        /// <summary>Converts the value of the current <see cref="W3CDateTime"/> object to Coordinated Universal Time (UTC).</summary>
        /// <returns>A <see cref="W3CDateTime"/> object whose <see cref="P:System.DateTime.Kind"></see> property is <see cref="F:System.DateTimeKind.Utc"></see>, and whose value is the UTC equivalent to the value of the current <see cref="W3CDateTime"/> object, or <see cref="F:System.DateTime.MaxValue"></see> if the converted value is too large to be represented by a <see cref="W3CDateTime"/> object, or <see cref="F:System.DateTime.MinValue"></see> if the converted value is too small to be represented by a <see cref="W3CDateTime"/> object.</returns>
        public W3CDateTime ToUniversalTime()
        {
            return new W3CDateTime(this.UtcTime, TimeSpan.Zero);
        }

        /// <summary>Converts the value of this instance to its equivalent string representation using the specified format.</summary>
        /// <returns>A string representation of value of this instance as specified by format.</returns>
        /// <param name="format">Format is "X" (RFC822) or "W" (W3C).</param>
        /// <exception cref="T:System.FormatException">The length of format is 1, and it is not one of the specified format characters defined for <see cref="T:System.Globalization.DateTimeFormatInfo"></see>.-or- format does not contain a valid custom format pattern. </exception>
        public string ToString(string format)
        {
            switch (format)
            {
                case "X":
                    return this.localDate.ToString(@"ddd, dd MMM yyyy HH:mm:ss ", CultureInfo.CurrentCulture) + FormatOffset(this.offset, string.Empty);

                case "W":
                    return this.localDate.ToString(@"yyyy-MM-ddTHH:mm:ss.FF", CultureInfo.CurrentCulture) + FormatOffset(this.offset, ":");

                default:
                    throw new FormatException("Unknown format specified.");
            }
        }

        /// <summary>Converts the value of this instance to its equivalent string representation.</summary>
        /// <returns>A string representation of value of this instance.</returns>
        public override string ToString()
        {
            return this.ToString("W");
        }

        private static int ParseRfc822Month(string monthName)
        {
            for (int i = 0; i < 12; i++)
            {
                if (monthName == MonthNames[i])
                {
                    return i + 1;
                }
            }

            throw new ArgumentException("Invalid month name");
        }

        private static TimeSpan ParseRfc822Offset(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return TimeSpan.Zero;
            }

            int hours = 0;
            switch (s)
            {
                case "UT":
                case "GMT":
                    break;
                case "EDT":
                    hours = -4;
                    break;
                case "EST":
                case "CDT":
                    hours = -5;
                    break;
                case "CST":
                case "MDT":
                    hours = -6;
                    break;
                case "MST":
                case "PDT":
                    hours = -7;
                    break;
                case "PST":
                    hours = -8;
                    break;
                default:
                    if (s[0] == '+')
                    {
                        string sfmt = s.Substring(1, 2) + ":" + s.Substring(3, 2);
                        return TimeSpan.Parse(sfmt);
                    }

                    return TimeSpan.Parse(s.Insert(s.Length - 2, ":"));
            }

            return TimeSpan.FromHours(hours);
        }

        private static TimeSpan ParseW3COffset(string s)
        {
            if (string.IsNullOrEmpty(s) || (s == "Z"))
            {
                return TimeSpan.Zero;
            }

            return s[0] == '+' ? TimeSpan.Parse(s.Substring(1)) : TimeSpan.Parse(s);
        }

        private static string FormatOffset(TimeSpan ofs, string separator)
        {
            string s = "-";
            if (ofs >= TimeSpan.Zero)
            {
                s = "+";
            }

            return s + Math.Abs(ofs.Hours).ToString("00", CultureInfo.CurrentCulture) + separator +
                           Math.Abs(ofs.Minutes).ToString("00", CultureInfo.CurrentCulture);
        }
    }
}