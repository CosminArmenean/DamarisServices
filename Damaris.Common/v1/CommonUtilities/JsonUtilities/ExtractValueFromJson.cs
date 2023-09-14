using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                // Check if the "LoginRequest" object exists in the JSON
                if (jsonObject.TryGetValue("LoginRequest", out var loginRequest) && loginRequest.Type == JTokenType.Object)
                {
                    // Check if the "RequestType" attribute exists within the "LoginRequest" object
                    if (loginRequest[attribute] is JValue requestType && requestType.Type == JTokenType.String)
                    {
                        return requestType.Value<string>();
                    }
                }

                // If any condition fails, return "unknown"
                return "unknown";
            }
            catch (JsonReaderException ex)
            {
                // Handle JSON parsing errors, if any
                Console.WriteLine($"JSON parsing error: {ex.Message}");
                return "Error"; // You can customize the error value
            }
        }

    }
}
