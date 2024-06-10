using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MAUI.API.Services;
public class TokenService(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration) =>
        new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            IssuerSigningKey = GetSecurityKey(configuration),
        };

    public string GenerateJwt(Guid userId, string userName)
    {
        var securityKey = GetSecurityKey(_configuration);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var issuer = _configuration["Jwt:Issuer"];

        var expireInMinutes = Convert.ToInt32(_configuration["Jwt:ExpireInMinute"]);

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            ];

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: "*",
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireInMinutes),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
            
    }

    private static SymmetricSecurityKey GetSecurityKey(IConfiguration configuration)
    {
        var secretkey = configuration["Jwt:SecretKey"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey!));

        return securityKey;
    }
}
