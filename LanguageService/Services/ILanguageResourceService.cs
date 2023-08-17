namespace LanguageService.Services
{
    public interface ILanguageResourceService
    {
        Dictionary<string, string> GetResource(string culture);
        void UpdateResource(string culture, Dictionary<string, string> updatedResource);
    }
}
