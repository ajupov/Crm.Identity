using System.Collections.Generic;

namespace Crm.Identity.Utils.Phone
{
    public static class PhoneUtils
    {
        public static Dictionary<string, PhoneSettings> Settings = new Dictionary<string, PhoneSettings>
        {
            {Country.Russia, new PhoneSettings(Country.Russia, 10, "7", "8")}
        };

        public static string GetInternationalPrefix(string country)
        {
            return Settings[country].InternationalPrefix;
        }

        public static string GetInnerPrefix(string country)
        {
            return Settings[country].InnerPrefix;
        }

        public static string GetFullInternationalPrefix(string country)
        {
            return $"{PhoneSettings.Plus}{GetInternationalPrefix(country)}";
        }

        public static string GetPhoneWithoutPrefixes(this string value, string country)
        {
            var settings = Settings[country];

            var fullInternationalPrefix = GetFullInternationalPrefix(country);
            if (value.Length == settings.Length + fullInternationalPrefix.Length &&
                value.StartsWith(fullInternationalPrefix))
            {
                return value.Substring(fullInternationalPrefix.Length);
            }

            var internationalPrefix = GetInternationalPrefix(country);
            if (value.Length == settings.Length + internationalPrefix.Length && value.StartsWith(internationalPrefix))
            {
                return value.Substring(internationalPrefix.Length);
            }

            var innerPrefix = GetInnerPrefix(country);
            if (value.Length == settings.Length + innerPrefix.Length && value.StartsWith(innerPrefix))
            {
                return value.Substring(innerPrefix.Length);
            }

            return value;
        }
    }
}