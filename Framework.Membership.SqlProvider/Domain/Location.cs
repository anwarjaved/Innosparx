﻿namespace Framework.Domain
{
    using System.Data.Entity.Spatial;
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Location.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class Location : IValueObject
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the Location class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public Location()
        {
            this.Address1 = string.Empty;
            this.Address2 = string.Empty;
            this.City = string.Empty;
            this.State = string.Empty;
            this.ZipCode = string.Empty;
            this.Country = string.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the address 1.
        /// </summary>
        ///
        /// <value>
        ///     The address 1.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Address1 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the address 2.
        /// </summary>
        ///
        /// <value>
        ///     The address 2.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Address2 { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the city.
        /// </summary>
        ///
        /// <value>
        ///     The city.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string City { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the state.
        /// </summary>
        ///
        /// <value>
        ///     The state.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string State { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the zip code.
        /// </summary>
        ///
        /// <value>
        ///     The zip code.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ZipCode { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the country.
        /// </summary>
        ///
        /// <value>
        ///     The total number of ry.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Country { get; set; }
/*
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the geo address.
        /// </summary>
        ///
        /// <value>
        ///     The geo address.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public DbGeography GeoAddress { get; set; }*/
    }
}
