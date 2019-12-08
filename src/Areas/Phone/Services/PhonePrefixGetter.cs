namespace Crm.Identity.Areas.Phone.Services
{
    public class PhonePrefixGetter : IPhonePrefixGetter
    {
        public string GetFull()
        {
            return "+7";
        }

        public string GetShort()
        {
            return "7";
        }
    }
}