using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.Domain.v1.Models.Culture
{
    public class SupportedCulture
    {
        public string CultureCode { get; set; }
        public string DisplayName { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string NumericFormat { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        // Add more regional settings properties as needed

        public SupportedCulture(string cultureCode, string displayName, string dateFormat, string timeFormat, string numericFormat, string currencyCode, string currencySymbol)
        {
            CultureCode = cultureCode;
            DisplayName = displayName;
            DateFormat = dateFormat;
            TimeFormat = timeFormat;
            NumericFormat = numericFormat;
            CurrencyCode = currencyCode;
            CurrencySymbol = currencySymbol;
        }
    }
}
