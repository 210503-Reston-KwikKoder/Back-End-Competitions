using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CBERest
{
    public class CheckScopeHandle : AuthorizationHandler<CheckScopeAuth>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckScopeAuth requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');
            if (scopes.Any(s => s == requirement.Scope)) context.Succeed(requirement);

            return Task.CompletedTask;

        }
    }
}
