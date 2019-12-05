using System;

namespace Crm.Identity.Areas.OAuth.Models.ChangePhone
{
    public class PostChangePhoneResponse
    {
        public PostChangePhoneResponse(Guid tokenId)
        {
            TokenId = tokenId;
            IsInvalidCredentials = false;
        }

        public PostChangePhoneResponse(bool isInvalidCredentials)
        {
            IsInvalidCredentials = isInvalidCredentials;
        }

        public Guid TokenId { get; }

        public bool IsInvalidCredentials { get; }
    }
}