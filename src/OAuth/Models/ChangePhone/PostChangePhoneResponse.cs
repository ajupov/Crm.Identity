using System;

namespace Ajupov.Identity.src.OAuth.Models.ChangePhone
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