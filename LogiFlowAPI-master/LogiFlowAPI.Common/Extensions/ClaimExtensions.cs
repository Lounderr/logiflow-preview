namespace LogiFlowAPI.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;

    public static class ClaimExtensions
    {
        public static string GetJti(this IEnumerable<Claim> claims)
            => claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti).Value;

        public static string GetId(this IEnumerable<Claim> claims)
            => claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

        public static string GetUserName(this IEnumerable<Claim> claims)
            => claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;

        public static string GetEmail(this IEnumerable<Claim> claims)
            => claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email).Value;

        public static DateTime GetIssuedAt(this IEnumerable<Claim> claims)
            => DateTime.UnixEpoch.AddSeconds(long.Parse(claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Iat).Value));

        public static DateTime GetExpiration(this IEnumerable<Claim> claims)
            => DateTime.UnixEpoch.AddSeconds(long.Parse(claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value));
    }
}
