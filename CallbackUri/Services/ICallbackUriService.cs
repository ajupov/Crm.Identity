﻿namespace Crm.Identity.CallbackUri.Services
{
    public interface ICallbackUriService
    {
        string GetByCode(string redirectUri, string state, string code);

        string GetByTokens(string redirectUri, string state, string accessToken, string refreshToken);
    }
}