namespace FacebookSDK
{
    using Framework.Rest;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An individual entry in a profile's feed as represented in the Graph API.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/14/2013 12:03 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class Post : BaseRequest
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        ///
        /// <value>
        ///     The message.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Message { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the picture.
        /// </summary>
        ///
        /// <value>
        ///     If available, a link to the picture included with this post
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Picture { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the link.
        /// </summary>
        ///
        /// <value>
        ///     The link attached to this post.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Link { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        ///
        /// <value>
        ///     The name of the link
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the caption.
        /// </summary>
        ///
        /// <value>
        ///    The caption of the link (appears beneath the link name)
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Caption { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        ///
        /// <value>
        ///     A description of the link (appears beneath the link caption).
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Description { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets source for the.
        /// </summary>
        ///
        /// <value>
        ///     A URL to a Flash movie or video file to be embedded within the post.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Source { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the tags.
        /// </summary>
        ///
        /// <value>
        ///     Objects (Users, Pages, etc) tagged as being with the publisher of the post
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string[] Tags { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the place.
        /// </summary>
        ///
        /// <value>
        ///     Location associated with a Post, if any
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Place { get; set; }

        protected internal override void AddFields(RestRequest request)
        {
            request.AddBody("message", this.Message);

            this.AddFieldIfNotEmpty(request, "tags", this.Tags);
            this.AddFieldIfNotEmpty(request, "place", this.Place);

            this.AddFieldIfNotEmpty(request, "picture", this.Picture);
            this.AddFieldIfNotEmpty(request, "link", this.Link);
            this.AddFieldIfNotEmpty(request, "name", this.Name);
            this.AddFieldIfNotEmpty(request, "caption", this.Caption);
            this.AddFieldIfNotEmpty(request, "description", this.Description);
            this.AddFieldIfNotEmpty(request, "source", this.Source);
        }

     
    }
}
