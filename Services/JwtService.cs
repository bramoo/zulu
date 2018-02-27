using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using zulu.Extensions;

namespace zulu.Services
{
  public static class JwtClaimIdentifiers
  {
    public const string Rol = "rol", Id = "id";
  }


  public static class JwtClaims
  {
    public const string ApiAccess = "api_access";
  }


  public interface IJwtService
  {
    Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
  }


  public class JwtService : IJwtService
  {
    private JwtIssuerOptions _jwtIssuerOptions;

    public JwtService(IOptions<JwtIssuerOptions> jwtOptions)
    {
      _jwtIssuerOptions = jwtOptions.Value;
    }


    public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
    {
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Sub, userName),
        new Claim(JwtRegisteredClaimNames.Jti, await _jwtIssuerOptions.JtiGenerator()),
        new Claim(JwtRegisteredClaimNames.Iat, _jwtIssuerOptions.IssuedAt.ToUnixEpoch().ToString(), ClaimValueTypes.Integer64),
        identity.FindFirst(JwtClaimIdentifiers.Rol),
        identity.FindFirst(JwtClaimIdentifiers.Id)
      };

      var token = new JwtSecurityToken(
          issuer: _jwtIssuerOptions.Issuer,
          audience: _jwtIssuerOptions.Audience,
          claims: claims,
          notBefore: _jwtIssuerOptions.NotBefore,
          expires: _jwtIssuerOptions.Expiration,
          signingCredentials: _jwtIssuerOptions.SigningCredentials);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
    //{
    //  return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
    //  {
    //            new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, id),
    //            new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Helpers.Constants.Strings.JwtClaims.ApiAccess)
    //        });
    //}
  }
}
