namespace MaxmindSDK
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;

    using Framework;

    /// <summary>
	/// Class to find out contry geoip info.
	/// </summary>
	/// <remarks>Code adapted from MaxMind http://www.maxmind.com/app/csharp with enhancements.</remarks>
	public sealed class LookupService : DisposableObject
	{
		private static readonly Country UnknownCountry = new Country("--", "N/A");

		private byte databaseType = Convert.ToByte(DatabaseType.CountryEdition);
		private const int CountryBegin = 16776960;
        private DatabaseInfo databaseInfo = null;

		private const int StructureInfoMaxSize = 20;

        private const int DatabaseInfoMaxSize = 100;

        private const int FullRecordLength = 100; //???

        private const int SegmentRecordLength = 3;

		private const int StandardRecordLength = 3;

		private const int OrgRecordLength = 4;

        private const int MaxRecordLength = 4;

        private const int MaxOrgRecordLength = 1000; //???

        private const int FipsRange = 360;

        private const int StateBeginRev0 = 16700000;

		private const int StateBeginRev1 = 16000000;

        private const int UsOffset = 1;

        private const int CanadaOffset = 677;

        private const int WorldOffset = 1353;

        public const int GeoIPUnknownSpeed = 0;
        public const int GeoIPDialupSpeed = 1;
        public const int GeoIPCabledSlSpeed = 2;
        public const int GeoIPCorporateSpeed = 3;

		private static readonly string[] CountryCodes = {
															"--", "AP", "EU", "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AN",
															"AO", "AQ", "AR", "AS", "AT", "AU", "AW", "AZ", "BA", "BB", "BD",
															"BE", "BF", "BG", "BH", "BI", "BJ", "BM", "BN", "BO", "BR", "BS",
															"BT", "BV", "BW", "BY", "BZ", "CA", "CC", "CD", "CF", "CG", "CH",
															"CI", "CK", "CL", "CM", "CN", "CO", "CR", "CU", "CV", "CX", "CY",
															"CZ", "DE", "DJ", "DK", "DM", "DO", "DZ", "EC", "EE", "EG", "EH",
															"ER", "ES", "ET", "FI", "FJ", "FK", "FM", "FO", "FR", "FX", "GA",
															"GB", "GD", "GE", "GF", "GH", "GI", "GL", "GM", "GN", "GP", "GQ",
															"GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR", "HT",
															"HU", "ID", "IE", "IL", "IN", "IO", "IQ", "IR", "IS", "IT", "JM",
															"JO", "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW",
															"KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU",
															"LV", "LY", "MA", "MC", "MD", "MG", "MH", "MK", "ML", "MM", "MN",
															"MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY",
															"MZ", "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO", "NP", "NR",
															"NU", "NZ", "OM", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM",
															"PN", "PR", "PS", "PT", "PW", "PY", "QA", "RE", "RO", "RU", "RW",
															"SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK", "SL",
															"SM", "SN", "SO", "SR", "ST", "SV", "SY", "SZ", "TC", "TD", "TF",
															"TG", "TH", "TJ", "TK", "TM", "TN", "TO", "TL", "TR", "TT", "TV",
															"TW", "TZ", "UA", "UG", "UM", "US", "UY", "UZ", "VA", "VC", "VE",
															"VG", "VI", "VN", "VU", "WF", "WS", "YE", "YT", "RS", "ZA", "ZM",
															"ME", "ZW", "A1", "A2", "O1", "AX", "GG", "IM", "JE", "BL", "MF"
														};

		private static readonly string[] CountryNames = {
															"N/A", "Asia/Pacific Region", "Europe", "Andorra",
															"United Arab Emirates", "Afghanistan", "Antigua and Barbuda",
															"Anguilla", "Albania", "Armenia", "Netherlands Antilles",
															"Angola", "Antarctica", "Argentina", "American Samoa", "Austria",
															"Australia", "Aruba", "Azerbaijan", "Bosnia and Herzegovina",
															"Barbados", "Bangladesh", "Belgium", "Burkina Faso", "Bulgaria",
															"Bahrain", "Burundi", "Benin", "Bermuda", "Brunei Darussalam",
															"Bolivia", "Brazil", "Bahamas", "Bhutan", "Bouvet Island",
															"Botswana", "Belarus", "Belize", "Canada",
															"Cocos (Keeling) Islands",
															"Congo, The Democratic Republic of the",
															"Central African Republic", "Congo", "Switzerland",
															"Cote D'Ivoire", "Cook Islands", "Chile", "Cameroon", "China",
															"Colombia", "Costa Rica", "Cuba", "Cape Verde",
															"Christmas Island", "Cyprus", "Czech Republic", "Germany",
															"Djibouti", "Denmark", "Dominica", "Dominican Republic",
															"Algeria", "Ecuador", "Estonia", "Egypt", "Western Sahara",
															"Eritrea", "Spain", "Ethiopia", "Finland", "Fiji",
															"Falkland Islands (Malvinas)", "Micronesia, Federated States of",
															"Faroe Islands", "France", "France, Metropolitan", "Gabon",
															"United Kingdom", "Grenada", "Georgia", "French Guiana", "Ghana",
															"Gibraltar", "Greenland", "Gambia", "Guinea", "Guadeloupe",
															"Equatorial Guinea", "Greece",
															"South Georgia and the South Sandwich Islands", "Guatemala",
															"Guam", "Guinea-Bissau", "Guyana", "Hong Kong",
															"Heard Island and McDonald Islands", "Honduras", "Croatia",
															"Haiti", "Hungary", "Indonesia", "Ireland", "Israel", "India",
															"British Indian Ocean Territory", "Iraq",
															"Iran, Islamic Republic of", "Iceland", "Italy", "Jamaica",
															"Jordan", "Japan", "Kenya", "Kyrgyzstan", "Cambodia", "Kiribati",
															"Comoros", "Saint Kitts and Nevis",
															"Korea, Democratic People's Republic of", "Korea, Republic of",
															"Kuwait", "Cayman Islands", "Kazakhstan",
															"Lao People's Democratic Republic", "Lebanon", "Saint Lucia",
															"Liechtenstein", "Sri Lanka", "Liberia", "Lesotho", "Lithuania",
															"Luxembourg", "Latvia", "Libyan Arab Jamahiriya", "Morocco",
															"Monaco", "Moldova, Republic of", "Madagascar",
															"Marshall Islands", "Macedonia, the Former Yugoslav Republic of",
															"Mali", "Myanmar", "Mongolia", "Macau",
															"Northern Mariana Islands", "Martinique", "Mauritania",
															"Montserrat", "Malta", "Mauritius", "Maldives", "Malawi",
															"Mexico", "Malaysia", "Mozambique", "Namibia", "New Caledonia",
															"Niger", "Norfolk Island", "Nigeria", "Nicaragua", "Netherlands",
															"Norway", "Nepal", "Nauru", "Niue", "New Zealand", "Oman",
															"Panama", "Peru", "French Polynesia", "Papua New Guinea",
															"Philippines", "Pakistan", "Poland", "Saint Pierre and Miquelon",
															"Pitcairn", "Puerto Rico", "" + "Palestinian Territory, Occupied"
															, "Portugal", "Palau", "Paraguay", "Qatar", "Reunion", "Romania",
															"Russian Federation", "Rwanda", "Saudi Arabia", "Solomon Islands"
															, "Seychelles", "Sudan", "Sweden", "Singapore", "Saint Helena",
															"Slovenia", "Svalbard and Jan Mayen", "Slovakia", "Sierra Leone",
															"San Marino", "Senegal", "Somalia", "Suriname",
															"Sao Tome and Principe", "El Salvador", "Syrian Arab Republic",
															"Swaziland", "Turks and Caicos Islands", "Chad",
															"French Southern Territories", "Togo", "Thailand", "Tajikistan",
															"Tokelau", "Turkmenistan", "Tunisia", "Tonga", "Timor-Leste",
															"Turkey", "Trinidad and Tobago", "Tuvalu", "Taiwan",
															"Tanzania, United Republic of", "Ukraine", "Uganda",
															"United States Minor Outlying Islands", "United States",
															"Uruguay", "Uzbekistan", "Holy See (Vatican City State)",
															"Saint Vincent and the Grenadines", "Venezuela",
															"Virgin Islands, British", "Virgin Islands, U.S.", "Vietnam",
															"Vanuatu", "Wallis and Futuna", "Samoa", "Yemen", "Mayotte",
															"Serbia", "South Africa", "Zambia", "Montenegro", "Zimbabwe",
															"Anonymous Proxy", "Satellite Provider", "Other", "Aland Islands"
															, "Guernsey", "Isle of Man", "Jersey", "Saint Barthelemy",
															"Saint Martin"
														};

		private Stream dbStream;
		int[] databaseSegments;
		int recordLength;


		public LookupService(Stream stream, LookupOptions options = LookupOptions.Standard)
		{

			switch (options)
			{
				case LookupOptions.Cache:
					MemoryStream ms = new MemoryStream();
					stream.Seek(0, SeekOrigin.Begin);
					stream.CopyTo(ms);
					ms.Seek(0, SeekOrigin.Begin);
					this.dbStream = ms;
					stream.Close();
					break;
				default:
					this.dbStream = stream;
					break;
			}

			this.Initialize();
		}

		private void Initialize()
		{
		    int i;
		    byte[] delim = new byte[3];
			byte[] buf = new byte[SegmentRecordLength];
			this.recordLength = StandardRecordLength;
			//file.Seek(file.Length() - 3,SeekOrigin.Begin);
			this.dbStream.Seek(-3, SeekOrigin.End);
			for (i = 0; i < StructureInfoMaxSize; i++)
			{
				this.dbStream.Read(delim, 0, 3);
				if (delim[0] == 255 && delim[1] == 255 && delim[2] == 255)
				{
					this.databaseType = Convert.ToByte(this.dbStream.ReadByte());
					if (this.databaseType >= 106)
					{
						// Backward compatibility with databases from April 2003 and earlier
						this.databaseType -= 105;
					}
					// Determine the database type.
					if (this.databaseType == (byte)DatabaseType.RegionEditionRev0)
					{
						this.databaseSegments = new int[1];
						this.databaseSegments[0] = StateBeginRev0;
						this.recordLength = StandardRecordLength;
					}
					else if (this.databaseType == (byte)DatabaseType.RegionEditionRev1)
					{
						this.databaseSegments = new int[1];
						this.databaseSegments[0] = StateBeginRev1;
						this.recordLength = StandardRecordLength;
					}
					else if (this.databaseType == (byte)DatabaseType.CityEditionRev0 ||
						  this.databaseType == (byte)DatabaseType.CityEditionRev1 ||
						  this.databaseType == (byte)DatabaseType.OrgEdition ||
						  this.databaseType == (byte)DatabaseType.OrgEditionV6 ||
						  this.databaseType == (byte)DatabaseType.ISPEdition ||
						  this.databaseType == (byte)DatabaseType.ISPEditionV6 ||
						  this.databaseType == (byte)DatabaseType.AutonomousSystemNumberEdition ||
						  this.databaseType == (byte)DatabaseType.AutonomousSystemNumberEditionV6 ||
						  this.databaseType == (byte)DatabaseType.NetSpeedEditionRev1 ||
			  this.databaseType == (byte)DatabaseType.NetSpeedEditionRev1V6 ||
						  this.databaseType == (byte)DatabaseType.CityEditionRev0V6 ||
			  this.databaseType == (byte)DatabaseType.CityEditionRev1V6
						  )
					{
						this.databaseSegments = new int[1];
						this.databaseSegments[0] = 0;
						if (this.databaseType == (byte)DatabaseType.CityEditionRev0 ||
							this.databaseType == (byte)DatabaseType.CityEditionRev1 ||
							this.databaseType == (byte)DatabaseType.AutonomousSystemNumberEditionV6 ||
							this.databaseType == (byte)DatabaseType.NetSpeedEditionRev1 ||
					this.databaseType == (byte)DatabaseType.NetSpeedEditionRev1V6 ||
							this.databaseType == (byte)DatabaseType.CityEditionRev0V6 ||
						this.databaseType == (byte)DatabaseType.CityEditionRev1V6 ||
							this.databaseType == (byte)DatabaseType.AutonomousSystemNumberEdition
							)
						{
							this.recordLength = StandardRecordLength;
						}
						else
						{
							this.recordLength = OrgRecordLength;
						}
						this.dbStream.Read(buf, 0, SegmentRecordLength);
                        for (int j = 0; j < SegmentRecordLength; j++)
						{
							this.databaseSegments[0] += ((buf[j].UnsignedByteToInt()) << (j * 8));
						}
					}
					break;
				}

				//file.Seek(file.getFilePointer() - 4);
				this.dbStream.Seek(-4, SeekOrigin.Current);
				//file.Seek(file.position-4,SeekOrigin.Begin);
			}

			if ((this.databaseType == (byte)DatabaseType.CountryEdition) ||
			  (this.databaseType == (byte)DatabaseType.CountryEditionV6) ||
				(this.databaseType == (byte)DatabaseType.ProxyEdition) ||
				(this.databaseType == (byte)DatabaseType.NetSpeedEdition))
			{
				this.databaseSegments = new int[1];
				this.databaseSegments[0] = CountryBegin;
				this.recordLength = StandardRecordLength;
			}
		}

		public LookupService(string databaseFile, LookupOptions options = LookupOptions.Standard)
			: this(new FileStream(databaseFile, FileMode.Open, FileAccess.Read), options)
		{
		}

        protected override void DisposeResources()
        {
            try
            {
                this.dbStream.Close();
                this.dbStream = null;
            }
            catch (Exception) { }
        }

        public Country GetCountry(IPAddress ipAddress)
        {
            return this.GetCountry(BytestoLong(ipAddress.GetAddressBytes()));
        }

        public Country GetCountryV6(string ipAddress)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(ipAddress);
            }
            //catch (UnknownHostException e) {
            catch (Exception e)
            {
                Console.Write(e.Message);
                return UnknownCountry;
            }

            return this.GetCountryV6(addr);
        }

        public Country GetCountry(string ipAddress)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(ipAddress);
            }
            //catch (UnknownHostException e) {
            catch (Exception e)
            {
                Console.Write(e.Message);
                return UnknownCountry;
            }
            //  return getCountry(bytestoLong(addr.GetAddressBytes()));
            return this.GetCountry(BytestoLong(addr.GetAddressBytes()));
        }

        public Country GetCountryV6(IPAddress ipAddress)
        {
            if (this.dbStream == null)
            {
                //throw new IllegalStateException("Database has been closed.");
                throw new Exception("Database has been closed.");
            }

            if ((this.databaseType == (byte)DatabaseType.CityEditionRev1) |
            (this.databaseType == (byte)DatabaseType.CityEditionRev0))
            {
                Location l = this.GetLocation(ipAddress);
                return l == null ? UnknownCountry : new Country(l.CountryCode, l.CountryName);
            }

            int ret = this.SeekCountryV6(ipAddress) - CountryBegin;
            return ret == 0 ? UnknownCountry : new Country(CountryCodes[ret], CountryNames[ret]);
        }

        public Country GetCountry(long ipAddress)
        {
            if (this.dbStream == null)
            {
                //throw new IllegalStateException("Database has been closed.");
                throw new Exception("Database has been closed.");
            }
            if ((this.databaseType == (byte)DatabaseType.CityEditionRev1) |
            (this.databaseType == (byte)DatabaseType.CityEditionRev0))
            {
                Location l = this.GetLocation(ipAddress);

                return l == null ? UnknownCountry : new Country(l.CountryCode, l.CountryName);
            }
            
            int ret = this.SeekCountry(ipAddress) - CountryBegin;
            return ret == 0 ? UnknownCountry : new Country(CountryCodes[ret], CountryNames[ret]);
        }

        public int GetID(string ipAddress)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(ipAddress);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return 0;
            }
            return this.GetID(BytestoLong(addr.GetAddressBytes()));
        }

        public int GetID(IPAddress ipAddress)
        {

            return this.GetID(BytestoLong(ipAddress.GetAddressBytes()));
        }

        public int GetID(long ipAddress)
        {
            if (this.dbStream == null)
            {
                throw new Exception("Database has been closed.");
            }

            int ret = this.SeekCountry(ipAddress) - this.databaseSegments[0];
            return ret;
        }

        public DatabaseInfo DatabaseInfo
        {
            get
            {
                if (this.databaseInfo != null)
                {
                    return this.databaseInfo;
                }
                try
                {
                    // Synchronize since we're accessing the database file.
                    lock (this)
                    {
                        bool hasStructureInfo = false;
                        byte[] delim = new byte[3];
                        // Advance to part of file where database info is stored.
                        this.dbStream.Seek(-3, SeekOrigin.End);
                        for (int i = 0; i < StructureInfoMaxSize; i++)
                        {
                            this.dbStream.Read(delim, 0, 3);
                            if (delim[0] == 255 && delim[1] == 255 && delim[2] == 255)
                            {
                                hasStructureInfo = true;
                                break;
                            }
                        }
                        this.dbStream.Seek(-3, hasStructureInfo ? SeekOrigin.Current : SeekOrigin.End);
                        // Find the database info string.
                        for (int i = 0; i < DatabaseInfoMaxSize; i++)
                        {
                            this.dbStream.Read(delim, 0, 3);
                            if (delim[0] == 0 && delim[1] == 0 && delim[2] == 0)
                            {
                                byte[] dbInfo = new byte[i];
                                char[] dbInfo2 = new char[i];
                                this.dbStream.Read(dbInfo, 0, i);
                                for (int a0 = 0; a0 < i; a0++)
                                {
                                    dbInfo2[a0] = Convert.ToChar(dbInfo[a0]);
                                }
                                // Create the database info object using the string.
                                this.databaseInfo = new DatabaseInfo(new string(dbInfo2));
                                return this.databaseInfo;
                            }
                            this.dbStream.Seek(-4, SeekOrigin.Current);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    //e.printStackTrace();
                }

                return new DatabaseInfo(string.Empty);
            }
        }

        public Region GetRegion(IPAddress ipAddress)
        {
            return this.GetRegion(BytestoLong(ipAddress.GetAddressBytes()));
        }

        public Region GetRegion(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }

            return this.GetRegion(BytestoLong(addr.GetAddressBytes()));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Region GetRegion(long ipnum)
        {
            Region record = new Region();
            int seekRegion;
            switch (this.databaseType)
            {
                case (byte)DatabaseType.RegionEditionRev0:
                    {
                        seekRegion = this.SeekCountry(ipnum) - StateBeginRev0;
                        char[] ch = new char[2];
                        if (seekRegion >= 1000)
                        {
                            record.CountryCode = "US";
                            record.CountryName = "United States";
                            ch[0] = (char)(((seekRegion - 1000) / 26) + 65);
                            ch[1] = (char)(((seekRegion - 1000) % 26) + 65);
                            record.Name = new string(ch);
                        }
                        else
                        {
                            record.CountryCode = CountryCodes[seekRegion];
                            record.CountryName = CountryNames[seekRegion];
                            record.Name = "";
                        }
                    }
                    break;
                case (byte)DatabaseType.RegionEditionRev1:
                    {
                        seekRegion = this.SeekCountry(ipnum) - StateBeginRev1;
                        char[] ch = new char[2];
                        if (seekRegion < UsOffset)
                        {
                            record.CountryCode = "";
                            record.CountryName = "";
                            record.Name = "";
                        }
                        else if (seekRegion < CanadaOffset)
                        {
                            record.CountryCode = "US";
                            record.CountryName = "United States";
                            ch[0] = (char)(((seekRegion - UsOffset) / 26) + 65);
                            ch[1] = (char)(((seekRegion - UsOffset) % 26) + 65);
                            record.Name = new string(ch);
                        }
                        else if (seekRegion < WorldOffset)
                        {
                            record.CountryCode = "CA";
                            record.CountryName = "Canada";
                            ch[0] = (char)(((seekRegion - CanadaOffset) / 26) + 65);
                            ch[1] = (char)(((seekRegion - CanadaOffset) % 26) + 65);
                            record.Name = new string(ch);
                        }
                        else
                        {
                            record.CountryCode = CountryCodes[(seekRegion - WorldOffset) / FipsRange];
                            record.CountryName = CountryNames[(seekRegion - WorldOffset) / FipsRange];
                            record.Name = "";
                        }
                    }
                    break;
            }
            return record;
        }

        public Location GetLocation(IPAddress addr)
        {
            return this.GetLocation(BytestoLong(addr.GetAddressBytes()));
        }
        public Location GetLocationV6(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }

            return this.GetLocationV6(addr);
        }

        public Location GetLocation(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }

            return this.GetLocation(BytestoLong(addr.GetAddressBytes()));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Location GetLocationV6(IPAddress addr)
        {
            byte[] recordBuf = new byte[FullRecordLength];
            char[] recordBuf2 = new char[FullRecordLength];
            int recordBufOffset = 0;
            Location record = new Location();
            int strLength = 0;
            double latitude = 0, longitude = 0;

            try
            {
                int seekCountry = this.SeekCountryV6(addr);
                if (seekCountry == this.databaseSegments[0])
                {
                    return null;
                }
                int recordPointer = seekCountry + ((2 * this.recordLength - 1) * this.databaseSegments[0]);
                this.dbStream.Seek(recordPointer, SeekOrigin.Begin);
                this.dbStream.Read(recordBuf, 0, FullRecordLength);
                for (int a0 = 0; a0 < FullRecordLength; a0++)
                {
                    recordBuf2[a0] = Convert.ToChar(recordBuf[a0]);
                }
                // get country
                record.CountryCode = CountryCodes[(recordBuf[0]).UnsignedByteToInt()];
                record.CountryName = CountryNames[(recordBuf[0]).UnsignedByteToInt()];
                recordBufOffset++;

                // get region
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.Region = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += strLength + 1;
                strLength = 0;

                // get region_name
                record.RegionName = RegionName.GetRegionName(record.CountryCode, record.Region);

                // get city
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.City = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += (strLength + 1);
                strLength = 0;

                // get postal code
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.PostalCode = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += (strLength + 1);

                // get latitude
                int j;
                for (j = 0; j < 3; j++)
                    latitude += ((recordBuf[recordBufOffset + j]).UnsignedByteToInt() << (j * 8));
                record.Latitude = (float)latitude / 10000 - 180;
                recordBufOffset += 3;

                // get longitude
                for (j = 0; j < 3; j++)
                    longitude += ((recordBuf[recordBufOffset + j]).UnsignedByteToInt() << (j * 8));
                record.Longitude = (float)longitude / 10000 - 180;

                record.MetroCode = record.DMACode = 0;
                record.AreaCode = 0;
                if (this.databaseType == (byte)DatabaseType.CityEditionRev1
                  || this.databaseType == (byte)DatabaseType.CityEditionRev1V6)
                {
                    // get metro_code
                    int metroareaCombo = 0;
                    if (record.CountryCode == "US")
                    {
                        recordBufOffset += 3;
                        for (j = 0; j < 3; j++)
                            metroareaCombo += ((recordBuf[recordBufOffset + j]).UnsignedByteToInt() << (j * 8));
                        record.MetroCode = record.DMACode = metroareaCombo / 1000;
                        record.AreaCode = metroareaCombo % 1000;
                    }
                }
            }
            catch (IOException)
            {
                Console.Write("IO Exception while seting up segments");
            }
            return record;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Location GetLocation(long ipnum)
        {
            byte[] recordBuf = new byte[FullRecordLength];
            char[] recordBuf2 = new char[FullRecordLength];
            int recordBufOffset = 0;
            Location record = new Location();
            int strLength = 0;
            double latitude = 0, longitude = 0;

            try
            {
                int seekCountry = this.SeekCountry(ipnum);
                if (seekCountry == this.databaseSegments[0])
                {
                    return null;
                }
                int recordPointer = seekCountry + ((2 * this.recordLength - 1) * this.databaseSegments[0]);
                this.dbStream.Seek(recordPointer, SeekOrigin.Begin);
                this.dbStream.Read(recordBuf, 0, FullRecordLength);
                for (int a0 = 0; a0 < FullRecordLength; a0++)
                {
                    recordBuf2[a0] = Convert.ToChar(recordBuf[a0]);
                }
                // get country
                record.CountryCode = CountryCodes[(recordBuf[0]).UnsignedByteToInt()];
                record.CountryName = CountryNames[(recordBuf[0]).UnsignedByteToInt()];
                recordBufOffset++;

                // get region
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.Region = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += strLength + 1;
                strLength = 0;

                // get region_name
                record.RegionName = RegionName.GetRegionName(record.CountryCode, record.Region);

                // get city
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.City = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += (strLength + 1);
                strLength = 0;

                // get postal code
                while (recordBuf[recordBufOffset + strLength] != '\0')
                    strLength++;
                if (strLength > 0)
                {
                    record.PostalCode = new string(recordBuf2, recordBufOffset, strLength);
                }
                recordBufOffset += (strLength + 1);

                // get latitude
                int j;
                for (j = 0; j < 3; j++)
                    latitude += ((recordBuf[recordBufOffset + j]).UnsignedByteToInt() << (j * 8));
                record.Latitude = (float)latitude / 10000 - 180;
                recordBufOffset += 3;

                // get longitude
                for (j = 0; j < 3; j++)
                    longitude += ((recordBuf[recordBufOffset + j]).UnsignedByteToInt() << (j * 8));
                record.Longitude = (float)longitude / 10000 - 180;

                record.MetroCode = record.DMACode = 0;
                record.AreaCode = 0;
                if (this.databaseType == (byte)DatabaseType.CityEditionRev1)
                {
                    // get metro_code
                    int metroareaCombo = 0;
                    if (record.CountryCode == "US")
                    {
                        recordBufOffset += 3;
                        for (j = 0; j < 3; j++)
                            metroareaCombo += ((recordBuf[recordBufOffset + j]).UnsignedByteToInt() << (j * 8));
                        record.MetroCode = record.DMACode = metroareaCombo / 1000;
                        record.AreaCode = metroareaCombo % 1000;
                    }
                }
            }
            catch (IOException)
            {
                Console.Write("IO Exception while seting up segments");
            }
            return record;
        }

        public string GetOrg(IPAddress addr)
        {
            return this.GetOrg(BytestoLong(addr.GetAddressBytes()));
        }

        public string GetOrgV6(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
            //catch (UnknownHostException e) {
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
            return this.GetOrgV6(addr);
        }

        public string GetOrg(string str)
        {
            IPAddress addr;
            try
            {
                addr = IPAddress.Parse(str);
            }
            //catch (UnknownHostException e) {
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
            return this.GetOrg(BytestoLong(addr.GetAddressBytes()));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetOrgV6(IPAddress addr)
        {
            int strLength = 0;
            byte[] buf = new byte[MaxOrgRecordLength];
            char[] buf2 = new char[MaxOrgRecordLength];

            try
            {
                int seekOrg = this.SeekCountryV6(addr);
                if (seekOrg == this.databaseSegments[0])
                {
                    return null;
                }

                int recordPointer = seekOrg + (2 * this.recordLength - 1) * this.databaseSegments[0];
                this.dbStream.Seek(recordPointer, SeekOrigin.Begin);
                this.dbStream.Read(buf, 0, MaxOrgRecordLength);
                while (buf[strLength] != 0)
                {
                    buf2[strLength] = Convert.ToChar(buf[strLength]);
                    strLength++;
                }
                buf2[strLength] = '\0';
                return new string(buf2, 0, strLength);
            }
            catch (IOException)
            {
                Console.Write("IO Exception");
                return null;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetOrg(long ipnum)
        {
            int strLength = 0;
            byte[] buf = new byte[MaxOrgRecordLength];
            char[] buf2 = new char[MaxOrgRecordLength];

            try
            {
                int seekOrg = this.SeekCountry(ipnum);
                if (seekOrg == this.databaseSegments[0])
                {
                    return null;
                }

                int recordPointer = seekOrg + (2 * this.recordLength - 1) * this.databaseSegments[0];
                this.dbStream.Seek(recordPointer, SeekOrigin.Begin);
                this.dbStream.Read(buf, 0, MaxOrgRecordLength);
                while (buf[strLength] != 0)
                {
                    buf2[strLength] = Convert.ToChar(buf[strLength]);
                    strLength++;
                }
                buf2[strLength] = '\0';
                string orgBuf = new string(buf2, 0, strLength);
                return orgBuf;
            }
            catch (IOException)
            {
                Console.Write("IO Exception");
                return null;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private int SeekCountryV6(IPAddress ipAddress)
        {
            byte[] v6Vec = ipAddress.GetAddressBytes();
            byte[] buf = new byte[2 * MaxRecordLength];
            int[] x = new int[2];
            int offset = 0;
            for (int depth = 127; depth >= 0; depth--)
            {
                try
                {
                    this.dbStream.Seek(2 * this.recordLength * offset, SeekOrigin.Begin);
                    this.dbStream.Read(buf, 0, 2 * MaxRecordLength);
                }
                catch (IOException)
                {
                    Console.Write("IO Exception");
                }

                for (int i = 0; i < 2; i++)
                {
                    x[i] = 0;
                    for (int j = 0; j < this.recordLength; j++)
                    {
                        int y = buf[(i * this.recordLength) + j];
                        if (y < 0)
                        {
                            y += 256;
                        }
                        x[i] += (y << (j * 8));
                    }
                }


                int bnum = 127 - depth;
                int idx = bnum >> 3;
                int bMask = 1 << (bnum & 7 ^ 7);
                if ((v6Vec[idx] & bMask) > 0)
                {
                    if (x[1] >= this.databaseSegments[0])
                    {
                        return x[1];
                    }
                    offset = x[1];
                }
                else
                {
                    if (x[0] >= this.databaseSegments[0])
                    {
                        return x[0];
                    }
                    offset = x[0];
                }
            }

            // shouldn't reach here
            Console.Write("Error Seeking country while Seeking " + ipAddress);
            return 0;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private int SeekCountry(long ipAddress)
        {
            byte[] buf = new byte[2 * MaxRecordLength];
            int[] x = new int[2];
            int offset = 0;
            for (int depth = 31; depth >= 0; depth--)
            {
                try
                {
                    this.dbStream.Seek(2 * this.recordLength * offset, SeekOrigin.Begin);
                    this.dbStream.Read(buf, 0, 2 * MaxRecordLength);
                }
                catch (IOException)
                {
                    Console.Write("IO Exception");
                }
                for (int i = 0; i < 2; i++)
                {
                    x[i] = 0;
                    for (int j = 0; j < this.recordLength; j++)
                    {
                        int y = buf[(i * this.recordLength) + j];
                        if (y < 0)
                        {
                            y += 256;
                        }
                        x[i] += (y << (j * 8));
                    }
                }

                if ((ipAddress & (1 << depth)) > 0)
                {
                    if (x[1] >= this.databaseSegments[0])
                    {
                        return x[1];
                    }
                    offset = x[1];
                }
                else
                {
                    if (x[0] >= this.databaseSegments[0])
                    {
                        return x[0];
                    }
                    offset = x[0];
                }
            }

            // shouldn't reach here
            Console.Write("Error Seeking country while Seeking " + ipAddress);
            return 0;

        }

        private static long BytestoLong(byte[] address)
        {
            long ipnum = 0;
            for (int i = 0; i < 4; ++i)
            {
                long y = address[i];
                if (y < 0)
                {
                    y += 256;
                }
                ipnum += y << ((3 - i) * 8);
            }
            return ipnum;
        }
	}
}
