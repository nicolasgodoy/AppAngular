using AppAngular.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppAngular.Service.Servicios
{
    public class JwtService
    {
        private readonly AuthConfiguration _authConfiguration;
        private readonly UserManager<AspNetUsers> _userManager;
        public JwtService(AuthConfiguration authConfiguration,
            UserManager<AspNetUsers> userManager)
        {
            _authConfiguration = authConfiguration;
            _userManager = userManager;
        }

        public async Task<string> GenerateAccessToken(AspNetUsers user)
        {
            DateTime tokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_authConfiguration.ExpirationMinutes));

            ICollection<string> roles = await this._userManager.GetRolesAsync(user);

            var userClaims = (await _userManager.GetClaimsAsync(user)).ToList();

            string token = GenerateToken(user, roles, userClaims, tokenExpiration);
            return token;
        }

        private string GenerateToken(AspNetUsers user, ICollection<string> roles, List<Claim> claims, DateTime? expiration = null)
        {
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList());
            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            // ✅ Agregar claim personalizado "isSeller"
            if (roles.Contains("Vendedor"))
                claims.Add(new Claim("isSeller", "true"));

            if (roles.Contains("Admin"))
                claims.Add(new Claim("isAdmin", "true"));

            return this.GenerateToken(user, claims, expiration);
        }

        private string GenerateToken(AspNetUsers user, IEnumerable<Claim> claims, DateTime? expiration = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfiguration.JwtKey);
            claims ??= new List<Claim>();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration ?? DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            // Create jwt token.
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Write jwt token to string.
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, bool validateExpiration = true)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfiguration.JwtKey)),
                ValidateLifetime = validateExpiration
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException();
            return principal;
        }
    }
}
