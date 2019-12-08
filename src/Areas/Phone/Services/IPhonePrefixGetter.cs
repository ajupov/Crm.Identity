namespace Crm.Identity.Areas.Phone.Services
{
    public interface IPhonePrefixGetter
    {
        string GetFull();
        
        string GetShort();
    }
}