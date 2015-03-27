using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Web.Mvc
{
    using System.Dynamic;
    using System.IO;
    using System.Security;
    using System.Web.Mvc;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [SecurityCritical]
    public class DefaultModelBinder : System.Web.Mvc.DefaultModelBinder
    {
        private static readonly List<string> AllowedVerbs = new List<string>() { "POST", "PUT" };

        [SecurityCritical]
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // first make sure we have a valid context
            if (controllerContext != null && AllowedVerbs.Contains(controllerContext.HttpContext.Request.HttpMethod)
                && controllerContext.HttpContext.Request.ContentType.StartsWith(
                    "application/json", StringComparison.OrdinalIgnoreCase))
            {
                if (!(bindingContext.ModelType.IsValueType ||
                    bindingContext.ModelType == typeof(string)))
                {
                    // get a generic stream reader (get reader for the http stream)
                    StreamReader streamReader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
                    // convert stream reader to a JSON Text Reader
                    JsonTextReader jsonReader = new JsonTextReader(streamReader);
                    // tell JSON to read
                    if (jsonReader.Read())
                    {
                        // make a new Json serializer
                        JsonSerializer jsonSerializer = new JsonSerializer();
                        // add the dyamic object converter to our serializer
                        jsonSerializer.Converters.Add(new ExpandoObjectConverter());

                        // use JSON.NET to deserialize object to a dynamic (expando) object
                        Object jsonObject;
                        if (bindingContext.ModelType == typeof(object))
                        {
                            // if we start with a "[", treat this as an array
                            if (jsonReader.TokenType == JsonToken.StartArray)
                                jsonObject = jsonSerializer.Deserialize<List<ExpandoObject>>(jsonReader);
                            else
                                jsonObject = jsonSerializer.Deserialize<ExpandoObject>(jsonReader);
                        }
                        else
                        {
                            jsonObject = jsonSerializer.Deserialize(jsonReader, bindingContext.ModelType);
                        }

                        return jsonObject;

                    }
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
