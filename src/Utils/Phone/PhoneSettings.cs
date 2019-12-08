namespace Crm.Identity.Utils.Phone
{
    public class PhoneSettings
    {
        public PhoneSettings(string country, int length, string internationalPrefix, string innerPrefix)
        {
            Country = country;
            Length = length;
            InternationalPrefix = internationalPrefix;
            InnerPrefix = innerPrefix;
        }
   
        public const string Plus = "+";

        public string Country { get; set; }

        public int Length { get; set; }

        public string InternationalPrefix { get; set; }

        public string InnerPrefix { get; set; }
    }
}