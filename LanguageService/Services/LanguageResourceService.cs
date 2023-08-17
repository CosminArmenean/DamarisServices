using Newtonsoft.Json;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace LanguageService.Services
{
    public class LanguageResourceService : ILanguageResourceService
    {
        private readonly string _languageResourcesPath;

        public LanguageResourceService()
        {
            _languageResourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "LanguageResources");
        }

        public Dictionary<string, string> GetResource(string culture)
        {
            var filePath = Path.Combine(_languageResourcesPath, "v1", $"{culture}.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }

            // Return a default resource or throw an exception
            return new Dictionary<string, string>();
        }

        public void UpdateResource(string culture, Dictionary<string, string> updatedResource)
        {
            var filePath = Path.Combine(_languageResourcesPath, $"{culture}.json");

            var json = JsonConvert.SerializeObject(updatedResource, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        public string SerializeResource(Dictionary<string, string> resource)
        {
            // For demonstration purposes, using JSON serialization
            return JsonConvert.SerializeObject(resource);
        }

        #region Private Functions

        #endregion //end private functions
    }
}
