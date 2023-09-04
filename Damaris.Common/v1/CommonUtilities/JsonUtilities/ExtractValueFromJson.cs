using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Damaris.Common.v1.CommonUtilities.JsonUtilities
{
    public static class ExtractValueFromJson
    {
        /// <summary>
        /// extracting an attribute value from json serialized string
        /// </summary>
        /// <param name="serializedJson"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string ExtractAttributeValue(string serializedJson, string attribute)
        {
            try
            {
                // Deserialize the JSON to a JObject
                JObject jsonObject = JsonConvert.DeserializeObject<JObject>(serializedJson);

                // Use the attributeName to access the corresponding property in the JSON
                JToken attributeValue = jsonObject[attribute];
                // Check if the attribute exists and is a string
                if (attributeValue != null && attributeValue.Type == JTokenType.String)
                {
                    return attributeValue.Value<string>();
                }
                else
                {
                    // Attribute doesn't exist or is not a string
                    return "unknown";
                }
            }
            catch (System.Text.Json.JsonException ex)
            {
                // Handle JSON parsing errors, if any
                Console.WriteLine($"JSON parsing error: {ex.Message}");
                return "Error"; // You can customize the error value
            }
        }

    }
}
