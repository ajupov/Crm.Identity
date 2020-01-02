﻿namespace Crm.Identity.OAuth.Models.Authorize
{
    public class PostAuthorizeResponse
    {
        public PostAuthorizeResponse(string callbackUri, bool isInvalidCredentials)
        {
            CallbackUri = callbackUri;
            IsInvalidCredentials = isInvalidCredentials;
        }

        public string CallbackUri { get; }

        public bool IsInvalidCredentials { get; }
    }
}