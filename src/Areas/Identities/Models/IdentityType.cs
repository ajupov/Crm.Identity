﻿namespace Crm.Identity.Areas.Identities.Models
{
    public enum IdentityType : byte
    {
        LoginAndPassword = 1,
        EmailAndPassword = 2,
        PhoneAndPassword = 3
    }
}