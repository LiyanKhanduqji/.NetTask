using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    string ITokenService.CreateToken(AppUser user)
    {
        // Retrieve the Secret Key
        var tokenKey = config["TokenKey"] ?? throw new Exception("Can't Access Token Key");
        if (tokenKey.Length < 64) throw new Exception("Token Key is Short");


        // TokenKey string is converted into bytes and wrapped in a SymmetricSecurityKey
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        if (user.UserName == null) throw new Exception("No username for user!");

        // Define Claims : allows the server to recognize the user by his username
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName)
        };

        // SigningCredentials are created using the security key and the HMAC SHA512 encryption algorithm.
        // credentials ensure that the token can be validated as authentic, allowing the server to detect if it has been altered.
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


        // A SecurityTokenDescriptor object is set up with:
        // Subject: The claims about the user, like their username.
        // Expires: A date 7 days in the future, meaning the token will be invalid after this time.
        // Signing Credentials: The credentials ensure the token is signed and secure.

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };


        // JwtSecurityTokenHandler generates the actual JWT using the descriptor
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // WriteToken converts it into a string format (a long series of characters separated by dots).
        return tokenHandler.WriteToken(token);  // final token sent to the user. The user can then include this token in requests to prove they are authenticated
    }
}
