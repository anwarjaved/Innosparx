using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookSDK
{
    using Framework;
    using Framework.Ioc;
    using Framework.Rest;
    using Framework.Serialization;
    using Framework.Serialization.Json;

    public class Privacy : BaseRequest
    {
        public PrivacyType? Type { get; set; }

        public string[] Allow { get; set; }

        public string[] Deny { get; set; }

        protected internal override void AddFields(RestRequest request)
        {
            if (this.Type.HasValue)
            {
                IJsonSerializer serializer = Container.Get<IJsonSerializer>();
                switch (Type.Value)
                {
                      case PrivacyType.Custom:
                        if ((this.Allow != null && this.Allow.Length > 0) || (this.Deny != null && this.Deny.Length > 0))
                        {
                            request.AddBody("privacy", serializer.Serialize(new
                            {
                                value = Type.Value.ToDescription(),
                                allow = Allow != null && Allow.Length > 0 ? Allow.ToConcatenatedString(",") : null,
                                deny = Deny != null && Deny.Length > 0 ? Deny.ToConcatenatedString(",") : null

                            }, SerializationMode.Compact));
          
                        }
                     break;
                    default:
                        request.AddBody("privacy", serializer.Serialize(new
                                                                        {
                                                                            value = Type.Value.ToDescription()
                                                                        }, SerializationMode.Compact));
                        break;
                }
            }
        }
    }
}
