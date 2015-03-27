using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Models;

namespace Framework.Web.Api
{
    using System.IO;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Security;

    using Framework.Ioc;
    using Framework.Serialization;
    using Framework.Serialization.Json;

    [SecurityCritical]
    public class DataTableFormatter : MediaTypeFormatter 
    {
        public DataTableFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        [SecurityCritical]
        public override bool CanReadType(Type type)
        {
            return false;
        }

        [SecurityCritical]
        public override bool CanWriteType(Type type)
        {
            if (type == typeof(DataTableModel))
            {
                return true;
            }

            return false;
        }

        [SecurityCritical]
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                if (type == typeof(DataTableModel))
                {
                    DataTableModel model = value as DataTableModel;

                    IJsonSerializer serializer = Container.Get<IJsonSerializer>();

                    using (var writer = new StreamWriter(writeStream))
                    {
                        writer.Write(serializer.Serialize(model, SerializationMode.Compact, ValueHandlingMode.Include));
                        writer.Flush();
                        writer.Close();
                    }
                }
            });
        }
    }
}
