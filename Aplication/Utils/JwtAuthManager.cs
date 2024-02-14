using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Aplication.Utils
{
    public class JwtAuthManager
    {
        private readonly string Key;

        public JwtAuthManager(string Key)
        {
            this.Key = Key;
        }
        public string Authenticate(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user._id.ToString())
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
