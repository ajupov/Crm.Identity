namespace Crm.Identity.Utils.Phone
{
    public static class Country
    {
        public static bool IsValidCountry(this string value) => value == Russia;

        public const string Russia = "RU";
    }
}