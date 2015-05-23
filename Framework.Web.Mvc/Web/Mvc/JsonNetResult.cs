namespace Framework.Web.Mvc
{
    using System;
    using System.Security;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using Framework.Serialization.Json.Converters;

    using Newtonsoft.Json;

    
    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }

        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        public JsonNetResult()
        {
            this.SerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            this.SerializerSettings.Converters.Add(new GuidConverter());
            this.SerializerSettings.Converters.Add(new StringEnumConverter());
            this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        public Formatting Formatting { get; set; }
    
        /// <summary>Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.</summary>
        /// <param name="context">The context within which the result is executed.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context" /> parameter is null.</exception>
        
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "This request has been blocked because sensitive information could be disclosed to "
                    + "third party web sites when this is used in a GET request. To allow GET requests, "
                    + "set JsonRequestBehavior to AllowGet.");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/json; charset=utf-8";

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data == null)
            {
                this.Data = new { };
            }

            response.Write(JsonConvert.SerializeObject(this.Data, this.Formatting, this.SerializerSettings));
        }
    }
}
