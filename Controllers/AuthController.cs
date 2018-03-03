using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zulu.Data;
using zulu.Services;
using zulu.ViewModels;

namespace zulu.Controllers
{
	[Route("/api/v1/auth")]
  public class AuthController : Controller
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly JwtIssuerOptions _jwtOptions;


    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtService jwtService, IOptions<JwtIssuerOptions> jwtOptions)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _jwtService = jwtService;
      _jwtOptions = jwtOptions.Value;
    }


    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody]CredentialsViewModel model)
    {
      var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

      if (!result.Succeeded)
      {
        return BadRequest(); //TODO modelstate from result.
      }

      var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
      var claims = await _userManager.GetClaimsAsync(appUser);
      var identity = new ClaimsIdentity(claims);

      return Ok(new JwtToken
      {
        Id = appUser.Id,
        Token = await _jwtService.GenerateEncodedToken(appUser.UserName, identity),
        ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds
      });
    }


    [Authorize]
    [HttpGet("superSecretSignal")]
    public IActionResult SuperSecretSignal()
    {
      return Redirect("https://media.giphy.com/media/zLBQYkwGGQdJC/giphy.gif");
    }


    [Authorize]
    [HttpGet("secretSquirrel")]
    public IActionResult SecretSquirrel()
    {
      return Redirect("https://pensivesquirrel.files.wordpress.com/2013/09/secret-sqjuirrel-2.gif");
    }



    //private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
    //{
    //  if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
    //    return await Task.FromResult<ClaimsIdentity>(null);

    //  // get the user to verifty
    //  var userToVerify = await _userManager.FindByNameAsync(userName);

    //  if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

    //  // check the credentials
    //  if (await _userManager.CheckPasswordAsync(userToVerify, password))
    //  {
    //    return await Task.FromResult(_jwtService.GenerateClaimsIdentity(userName, userToVerify.Id));
    //  }

    //  // Credentials are invalid, or account doesn't exist
    //  return await Task.FromResult<ClaimsIdentity>(null);
    //}
  }
}
