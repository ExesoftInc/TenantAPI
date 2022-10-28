using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TenantAPI.Entities;

namespace TenantAPI.Authentication
{
    public class Policies
    {
        public const string Master = "Master";
        public const string Manager = "Manager";
        public const string Crew = "Crew";
        public const string User = "User";
        private EntitiesContext _context = new EntitiesContext();

        public static AuthorizationPolicy MasterPolicy()
        {

            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Master).Build();
        }

        public static AuthorizationPolicy AdminPolicy()
        {

            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(new string[] { Master, Manager}).Build();
        }

        public static AuthorizationPolicy CrewPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(new string[] { Master, Manager, Crew }).Build();
        }

        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(new string[] { Master, Manager, Crew, User }).Build();
        }
    }
}
