﻿using System.Collections.Generic;
using System.Linq;

namespace Crm.Identity.OAuth.Extensions
{
    public static class RequestScopeExtensions
    {
        public static List<string> ToList(this string scope)
        {
            return scope.Split(' ').ToList();
        }
    }
}