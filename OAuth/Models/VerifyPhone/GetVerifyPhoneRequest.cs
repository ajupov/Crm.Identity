﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Ajupov.Identity.OAuth.Models.VerifyPhone
{
    public class GetVerifyPhoneRequest
    {
        [Required]
        public Guid TokenId { get; set; }

        public string CallbackUri { get; set; }

        public bool IsInvalidCode { get; set; }
    }
}