namespace LogiFlowAPI.Services.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using LogiFlowAPI.Data.Common.Repositories;
    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Services.Interfaces;
    using LogiFlowAPI.Web.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class TokenService : ITokenService
    {
        private readonly UserManager<User> userManager;
        private readonly IDeletableEntityRepository<User> usersRepo;
        private readonly TokenSettings tokenSettings;
        private readonly IRepository<IdentityUserToken<string>> userTokens;

        public TokenService(
            UserManager<User> userManager,
            IDeletableEntityRepository<User> usersRepo,
            IOptions<TokenSettings> tokenSettings,
            IRepository<IdentityUserToken<string>> userTokens)
        {
            this.userManager = userManager;
            this.usersRepo = usersRepo;
            this.tokenSettings = tokenSettings.Value;
            this.userTokens = userTokens;
        }

        public IEnumerable<Claim> ExtractClaims(string token)
        {
            var tokenObj = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return tokenObj.Claims;
        }

        public async Task<string> GenerateAuthToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new("SecurityStamp", user.SecurityStamp),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow
                    .ToUnixTimeSeconds().ToString()),
                new(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow
                    .AddHours(this.tokenSettings.AuthTokenExpiryInMinutes)
                    .ToUnixTimeSeconds().ToString()),
            };

            var userRoles = await this.userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = new JwtSecurityToken(
                issuer: this.tokenSettings.Issuer,
                audience: this.tokenSettings.Audience,
                expires: DateTime.Now.AddHours(this.tokenSettings.AuthTokenExpiryInMinutes),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.tokenSettings.Secret)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshToken(string userId)
        {
            var randomBytes = new byte[32];
            RandomNumberGenerator.Fill(randomBytes);

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(randomBytes);

                await this.userTokens.AddAsync(new IdentityUserToken<string>
                {
                    UserId = userId,
                    LoginProvider = "Internal",
                    Name = "Refresh",
                    Value = Convert.ToBase64String(hashedBytes)
                });

                return Convert.ToBase64String(randomBytes);
            }
        }

        public async Task<bool> ValidateRefreshToken(string token)
        {
            var bytes = Convert.FromBase64String(token);

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(bytes);
                var hashedToken = Convert.ToBase64String(hashedBytes);

                return await this.userTokens.AnyAsync(t => t.Value == hashedToken);
            }
        }

        public async Task RevokeToken(string token)
        {
            var bytes = Convert.FromBase64String(token);
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(bytes);
                var hashedToken = Convert.ToBase64String(hashedBytes);
                var tokenToDelete = await this.userTokens.All().FirstOrDefaultAsync(t => t.Value == hashedToken);

                if (tokenToDelete != null)
                {
                    this.userTokens.Delete(tokenToDelete);
                    await this.userTokens.SaveChangesAsync();
                }
            }
        }

        public async Task RevokeAllRefreshTokens(string userId)
        {
            var tokensToDelete = this.userTokens.All().Where(t => t.UserId == userId);

            foreach (var token in tokensToDelete)
            {
                this.userTokens.Delete(token);
            }

            await this.userTokens.SaveChangesAsync();
        }
    }
}
