﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure.AuthorizationPolicy
{
    public class MinimumAgeRequirement:IAuthorizationRequirement
    {
        public int MinimumAge { get;  }
        public MinimumAgeRequirement( int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
