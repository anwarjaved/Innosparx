using System.Dynamic;

namespace GoogleSDK.Converters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Framework;

    using GoogleSDK.Places;

    using Newtonsoft.Json;

    public class AddressDetailsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                AddressDetails addressDetails = new AddressDetails();

                var list = new List<AddressComponent>();
                serializer.Populate(reader, list);

                foreach (var component in list)
                {
                    if (component.Types != null)
                    {
                        if (component.Types.Contains("street_number")){
                            addressDetails.Street = component.LongName;
                        }

                        if (component.Types.Contains("locality"))
                        {
                            addressDetails.City = component.LongName;
                        }

                        if (component.Types.Contains("administrative_area_level_1"))
                        {
                            addressDetails.State = component.LongName;
                        }

                        if (component.Types.Contains("country"))
                        {
                            addressDetails.Country = component.LongName;
                        }

                        if (component.Types.Contains("postal_code"))
                        {
                            addressDetails.Zip = component.LongName;
                        }
                    }
                }

                return addressDetails;
            }

            throw new JsonSerializationException("Unexpected token when parsing AddressDetails. Expected address_components object, got {0}.".FormatString(reader.TokenType));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AddressDetails);
        }
    }
}
