using Damaris.Domain.v1.Models.Culture;

namespace DamarisServices.SupportedCultures.v1
{
    public class SupportedCultureProvider
    {
        public IEnumerable<SupportedCulture> GetSupportedCultures()
        {
            // Fetch supported cultures from a configuration source (e.g., configuration file, database)
            // For simplicity, let's use hard-coded values here
            var cultures = new List<SupportedCulture>
            {
                 new SupportedCulture("en-US", "English (US)", "MM/dd/yyyy", "hh:mm tt", "0.00", "USD", "$"),
                 new SupportedCulture("ro-RO", "Romanian (ROMANIA)", "dd/MM/yyyy", "HH:mm", "0,00", "EUR", "€"),
                 // Add more supported cultures as needed               
            };

            return cultures;
        }
    }
}
