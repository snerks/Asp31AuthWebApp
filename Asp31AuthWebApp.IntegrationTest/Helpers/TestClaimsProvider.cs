using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Asp31AuthWebApp.IntegrationTest
{
    public class TestClaimsProvider
    {
        public IList<Claim> Claims
        {
            get;
        }

        public TestClaimsProvider(IList<Claim> claims)
        {
            Claims = claims;
        }

        public TestClaimsProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimsProvider WithAdminClaims()
        {
            var provider = new TestClaimsProvider(GetAdminClaims());
            //provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            //provider.Claims.Add(new Claim(ClaimTypes.Name, "Admin user"));
            //provider.Claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            return provider;
        }

        public static TestClaimsProvider WithUserClaims()
        {
            var provider = new TestClaimsProvider(GetUserClaims());
            //provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            //provider.Claims.Add(new Claim(ClaimTypes.Name, "User"));

            return provider;
        }

        public static IList<Claim> GetAdminClaims()
        {
            var results = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin")
            };

            results.AddRange(GetNameClaimsForName("Admin user"));

            return results;
        }

        public static IList<Claim> GetUserClaims()
        {
            return GetNameClaimsForName("User");
        }

        public static IList<Claim> GetNameClaimsForName(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, name),
            };
        }
    }
}
