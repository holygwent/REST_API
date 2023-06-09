﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Infrastructure.AuthorizationPolicy
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var userEmail = context.User.FindFirst(x => x.Type == ClaimTypes.Name).Value;
            var dateOfBirth =DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
            if (dateOfBirth.AddYears(requirement.MinimumAge) < DateTime.Today)
            {
                _logger.LogInformation($"User:{userEmail} with date of birth[{dateOfBirth}] passed age authorization");
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
