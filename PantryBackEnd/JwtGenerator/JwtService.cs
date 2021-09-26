using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System;
namespace PantryBackEnd.JwtGenerator
{
    public class JwtService
    {
        private string key = "Secure key this is extremely secure";

        public string Generator(Guid id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            //Expiration time of token
            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddHours(2));
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public JwtSecurityToken Verification(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Encoding.ASCII.GetBytes(key);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken token);
            return (JwtSecurityToken)token;
        }
    }
}